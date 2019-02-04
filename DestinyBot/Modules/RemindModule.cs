using System;
using System.Threading.Tasks;
using DestinyBot.Data;
using DestinyBot.Data.Entities;
using DestinyBot.Preconditions;
using DestinyBot.Services;
using Discord;
using Discord.Commands;

namespace DestinyBot.Modules
{
    public class RemindModule : ModuleBase<SocketCommandContext>
    {
        private readonly DestinyBotContext _db;
        private readonly ReminderService _reminder;

        public RemindModule(ReminderService reminder, DestinyBotContext db)
        {
            _reminder = reminder;
            _db = db;
        }

        [Command("reminder")]
        [Alias("rm", "remindme", "remind")]
        [ThrottleCommand]
        [RequireOwnerOrAdmin]
        public async Task Reminder(TimeSpan waitTime, [Remainder] string message)
        {
            var reminder = new Reminder
            {
                ChannelId = (long) Context.Channel.Id,
                DateCreated = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Message = message,
                TimeToRemind = (DateTimeOffset.UtcNow + waitTime).ToUnixTimeSeconds(),
                GuildId = (long) Context.Guild.Id,
                UserId = (long) Context.User.Id
            };
            var entity = _db.Reminders.Add(reminder).Entity;
            _db.SaveChanges();
            _db.Dispose();

            if (_reminder.AddReminder(entity))
                await Context.Message.AddReactionAsync(new Emoji("✅"));
            else
                await Context.Message.AddReactionAsync(new Emoji("❌"));
        }
    }
}