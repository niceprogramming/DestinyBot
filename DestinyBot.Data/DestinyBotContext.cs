﻿using System.IO;
using DestinyBot.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DestinyBot.Data
{
    public class DestinyBotContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public DestinyBotContext()
        {
        }

        public DestinyBotContext(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }
        public DbSet<TwitterUser> TwitterUsers { get; set; }
        public DbSet<TwitterSubscription> TwitterSubscriptions { get; set; }
        public DbSet<GuildOwner> GuildOwners { get; set; }
        public DbSet<Youtube> Channels { get; set; }
        public DbSet<TwitchStreamer> TwitchStreamers { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<YoutubeSubscription> YoutubeSubscriptions { get; set; }
        public DbSet<TwitchSubscription> TwitchSubscriptions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
            optionsBuilder.UseSqlite("Data Source=data/bot.db");

            optionsBuilder.UseLoggerFactory(_loggerFactory);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GuildOwner>(e => e.HasKey(_ => _.GuildId));
            modelBuilder.Entity<TwitterUser>(t =>
            {
                t.HasKey(x => x.Id);
                t.HasIndex(x => x.LastTweetId);

                t.HasMany(x => x.TwitterSubscriptions).WithOne(x => x.User).HasForeignKey(x => x.TwitterUserId);
            });

            modelBuilder.Entity<Youtube>(t =>
            {
                t.HasKey(x => x.Id);
                t.HasMany(x => x.YoutubeSubscriptions).WithOne(x => x.Youtube).HasForeignKey(x => x.YoutubeId);
                ;
            });

            modelBuilder.Entity<TwitchStreamer>(t =>
            {
                t.HasKey(x => x.Id);
                t.HasMany(x => x.Games).WithOne(x => x.TwitchStreamer).HasForeignKey(x => x.TwitchStreamerId);
                t.HasMany(x => x.TwitchSubscriptions).WithOne(x => x.TwitchStreamer)
                    .HasForeignKey(x => x.TwitchStreamerId);
            });
            modelBuilder.Entity<YoutubeSubscription>(e => e.HasKey(x => x.Id));
            modelBuilder.Entity<TwitchSubscription>(e => e.HasKey(_ => _.Id));
        }
    }
}