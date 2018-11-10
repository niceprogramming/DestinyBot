using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace DestinyBot
{
    public static class BotLogHook
    {
        public static Task Log(LogMessage message)
        {
            switch (message.Severity)
            {
                
                case LogSeverity.Critical:
                    Serilog.Log.Error(message.ToString());
                    break;
                case LogSeverity.Debug:
                    Serilog.Log.Debug(message.ToString());
                    break;
                case LogSeverity.Warning:
                    Serilog.Log.Warning(message.ToString());
                    break;
                case LogSeverity.Error:
                    Serilog.Log.Error(message.ToString());
                    break;
                case LogSeverity.Info:
                    Serilog.Log.Information(message.ToString());
                    break;
                case LogSeverity.Verbose:
                    Serilog.Log.Verbose(message.ToString());
                    break;
            }

            return Task.CompletedTask;
        }
    }
}
