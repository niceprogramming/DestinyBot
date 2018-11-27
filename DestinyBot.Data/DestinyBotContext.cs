using System;
using DestinyBot.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DestinyBot.Data
{
    public class DestinyBotContext : DbContext
    {
        public DbSet<GuildOwner> GuildOwners { get; set; }
        public DbSet<YoutubeSubscription> YoutubeSubscriptions { get; set; }

        public DestinyBotContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=bot.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<GuildOwner>(e => e.HasKey(_ => _.GuildId));
            modelBuilder.Entity<YoutubeSubscription>(e => e.HasKey(_ => new {_.ChannelId, _.DiscordChannelId}));
        }
    }
}
