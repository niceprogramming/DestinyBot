using System;
using System.Threading.Tasks;
using DestinyBot.Models;

namespace DestinyBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new DestinyBot().StartAsync();
        }
    }
}
