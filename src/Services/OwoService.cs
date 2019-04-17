using System;
using System.Threading.Tasks;
using DepressedBot.Data.Objects.EventArgs;
using Gommon;

namespace DepressedBot.Services
{
    [Service("OwO", "The main Service for OwO-ing your day. (please god end me)")]
    public sealed class OwoService
    {

        public async Task OnMessageReceivedAsync(MessageReceivedEventArgs args)
        {
            if (args.Message.Content.EqualsIgnoreCase("owo")
                || args.Message.Content.EqualsIgnoreCase("0w0"))
            {
                var rng = new BoolGenerator();
                for (var i = 2; rng.NextBool(); i *= 2)
                    await args.Context.ReplyAsync($"owo (1/{i})");

                await args.Context.ReplyAsync("whats this");
            }
        }

    }

    public sealed class BoolGenerator
    {
        private Random _rnd;

        public BoolGenerator()
        {
            _rnd = new Random();
        }

        public bool NextBool() => Convert.ToBoolean(_rnd.Next(0, 2));
    }
}
