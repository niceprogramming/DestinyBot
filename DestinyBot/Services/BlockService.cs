using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DestinyBot.Data;
using DestinyBot.Data.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace DestinyBot.Services
{
    public class BlockService
    {
        private IServiceProvider _provider;
        private ConcurrentDictionary<ulong, BlockedUser> userCache = new ConcurrentDictionary<ulong, BlockedUser>();

        public BlockService()
        {

        }

        public async Task StartAsync(IServiceProvider provider)
        {
            _provider = provider;
            using (var db = _provider.GetService<DestinyBotContext>())
            {
                userCache = new ConcurrentDictionary<ulong, BlockedUser>(db.BlockedUsers.ToDictionary(x => x.UserId));
            }
        }

        public async Task<bool> BlockUser(ulong userId)
        {
            if (userCache.TryGetValue(userId, out BlockedUser user))
            {
                return true;
            }

            var newUser = new BlockedUser() { UserId = userId };
            using (var db = _provider.GetService<DestinyBotContext>())
            {
                db.BlockedUsers.Add(newUser);
                userCache.TryAdd(userId, newUser);

                return (await db.SaveChangesAsync()) > 0;
            }


        }

        public async Task<bool> UnblockUser(ulong userId)
        {
            if (!userCache.Remove(userId, out BlockedUser user)) return false;
            using (var db = _provider.GetService<DestinyBotContext>())
            {
                db.BlockedUsers.Remove(user);

                var result = await db.SaveChangesAsync();
                return result > 0;
            }
        }

        public bool IsBlocked(ulong userId)
        {
            return userCache.TryGetValue(userId, out _);
        }
    }
}
