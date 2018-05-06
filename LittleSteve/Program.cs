using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Serilog;
using Serilog.Exceptions;
using Serilog.Formatting.Json;
using System.Linq;
using LittleSteve.Models;
using Newtonsoft.Json;

namespace LittleSteve
{
    internal class Program
    {

        private static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
               // .WriteTo.File(new JsonFormatter(renderMessage: true),"log.txt", rollingInterval: RollingInterval.Day)
                .Enrich.WithExceptionDetails()
                .CreateLogger();
           
            try
            {
                await new SteveBot().StartAsync();
            }
            catch (Exception e)
            {
                Log.Information(e, "Bot is donezo");
            }
        }
    }
}