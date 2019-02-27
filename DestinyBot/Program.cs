using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Serilog;

namespace DestinyBot
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            try
            {
                Directory.CreateDirectory("data");
                await new DestinyBot().StartAsync();
            }
            catch (Exception e)
            {
                // How real men prevent errors
                Log.Information(e,"Application exiting with Exception");
                throw;
            }
        }
    }
}