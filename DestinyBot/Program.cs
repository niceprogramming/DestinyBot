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
            try
            {
                await new DestinyBot().StartAsync();
            }
            catch (Exception e)
            {
                // How real men prevent errors
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}
