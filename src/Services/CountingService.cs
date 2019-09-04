using System;
using System.Threading.Tasks;
using DepressedBot.Data.Objects.EventArgs;
using Gommon;

namespace DepressedBot.Services
{
    [Service("Counting", "The main Service for handling the counting channel.")]
    public sealed class CountingService
    {
        public async Task OnMessageReceivedAsync(MessageReceivedEventArgs args)
        {
            if (args.Context.User.Id == args.Context.Client.CurrentUser.Id) return;
            if (args.Context.Channel.Id != 618651341106970664) return;
            if (!long.TryParse(args.Message.Content, out _))
            {
                await args.Message.DeleteAsync();
                var m = await args.Context.ReplyAsync($"{args.Message.Author.Mention}, only send numbers in this channel.");
                await Executor.ExecuteAfterDelayAsync(TimeSpan.FromSeconds(3), () => m.DeleteAsync());
            }
                
        }
    }
}
