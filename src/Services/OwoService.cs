using System;
using System.Threading.Tasks;
using DepressedBot.Data.Objects.EventArgs;
using Gommon;

namespace DepressedBot.Services
{
    [Service("OwO", "The main Service for OwO-ing your day. (please god end me)")]
    public sealed class OwoService
    {
        private DatabaseService _db;

        public OwoService(DatabaseService databaseService)
        {
            _db = databaseService;
        }

        public async Task OnMessageReceivedAsync(MessageReceivedEventArgs args)
        {
            if (args.Message.Content.EqualsIgnoreCase("owo")
                || args.Message.Content.EqualsIgnoreCase("0w0"))
            {
                int i = 0;
                for (i = 2; NextBool(); i *= 2)
                    await args.Context.ReplyAsync($"owo (1/{i})");

                await args.Context.ReplyAsync("whats this");
                var u = _db.GetUser(args.Context.User.Id);
                if (u.OwoScore > i/2) return;
                u.OwoScore = i/2;
                _db.UpdateUser(u);

            }
        }

        private bool NextBool() => Convert.ToBoolean(new Random().Next(0, 2));
    }
}
