using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DestinyBot.Data;
using DestinyBot.Data.Entities;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DestinyBot.Services
{
    public class ReminderService
    {
        private readonly DiscordSocketClient _client;
        private IServiceProvider _provider;

        public ReminderService(DiscordSocketClient client)
        {
            _client = client;
        }

        public ConcurrentDictionary<int, Timer> Reminders { get; } = new ConcurrentDictionary<int, Timer>();

        public Task StartAsync(IServiceProvider provider)
        {
            _provider = provider;
            using (var db = provider.GetService<DestinyBotContext>())
            {
                var reminders = db.Reminders.ToList();
                foreach (var reminder in reminders) AddReminder(reminder);
            }

            return Task.CompletedTask;
        }

        public bool AddReminder(Reminder reminder)
        {
            var current = DateTimeOffset.FromUnixTimeSeconds(reminder.TimeToRemind) - DateTimeOffset.UtcNow;

            if (current < TimeSpan.Zero) RemoveReminder(reminder);

            var timer = new Timer(ReminderCallback, reminder, current, TimeSpan.FromMilliseconds(-1));

            if (!Reminders.TryAdd(reminder.Id, timer))
            {
                RemoveReminder(reminder);
                return false;
            }

            Log.Information("Created new reminder {@reminder}", reminder);
            return true;
        }

        private void ReminderCallback(object reminderObject)
        {
            var reminder = reminderObject as Reminder;

            var channel = _client.GetGuild((ulong)reminder.GuildId)?.GetTextChannel((ulong)reminder.ChannelId);

            var embed = new EmbedBuilder().WithTitle("Reminder")
                .WithDescription(reminder.Message)
                .AddField("Created At", DateTimeOffset.FromUnixTimeSeconds(reminder.DateCreated))
                .Build();

            var message = channel?.SendMessageAsync(embed: embed, text: $"<@{reminder.UserId}>").GetAwaiter()
                .GetResult();
            Log.Information("Timer expired for Reminder {id}. Message {message} created", reminder.Id, message?.Id);
            RemoveReminder(reminder);
        }

        private void RemoveReminder(Reminder reminder)
        {
            using (var db = _provider.GetService<DestinyBotContext>())
            {
                if (Reminders.TryRemove(reminder.Id, out var t)) t.Dispose();

                db.Reminders.Remove(reminder);
                db.SaveChanges();
                Log.Information("Reminder {@reminder} removed", reminder);
            }
        }
    }
}