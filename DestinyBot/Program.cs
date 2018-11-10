using System;
using System.Threading.Tasks;
using DestinyBot.Models;
using Serilog;

namespace DestinyBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            await new DestinyBot().StartAsync();
        }
    }
}
