using System.Collections.Generic;
using System.Threading.Tasks;
using DestinyBot.Preconditions;
using Discord.Commands;

namespace DestinyBot.Modules
{
    public class EightBallModule : ModuleBase<SocketCommandContext>
    {
        [Command("8ball")]
        [NotBlockedPrecondtion]
        [ThrottleCommand]
        public async Task EightBall([Remainder] string question)
        {
            var answers = new List<string>
            {
                "It is certain.",
                "It is decidedly so.",
                "Without a doubt.",
                "Yes - definitely.",
                "You may rely on it.",
                "As I see it, yes.",
                "Most likely.",
                "Outlook good.",
                "Yes.",
                "Signs point to yes.",
                "Reply hazy, try again.",
                "Ask again later.",
                "Better not tell you now.",
                "Cannot predict now.",
                "Concentrate and ask again.",
                "Don't count on it.",
                "My reply is no.",
                "My sources say no.",
                "Outlook not so good.",
                "Very doubtful."
            };

            await ReplyAsync(answers.Random());
        }
    }
}