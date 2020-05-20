using System;
using System.Linq;
using System.Threading.Tasks;
using DepressedBot.Data.Objects.EventArgs;
using Discord;
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
            if (!long.TryParse(args.Message.Content, out var value))
            {
                await DoDeleteAsync(args);
                return;
            }

            var previousMessage = (await args.Context.Channel.GetMessagesAsync(args.Context.Message, Direction.Before, 1).FlattenAsync()).First();
            if (value != long.Parse(previousMessage.Content) + 1)
            {
                await DoDeleteAsync(args);
                return;
            }

            if (previousMessage.Author.Id == args.Context.Message.Author.Id)
            {
                await DoDeleteAsync(args);
            }

        }

        private async Task DoDeleteAsync(MessageReceivedEventArgs args)
        {
            await args.Message.DeleteAsync();
            var m = await args.Context.ReplyAsync($"{args.Message.Author.Mention}, only send numbers in this channel, that are exactly +1 of the previous number, and you are not the sender of the previous number.");
            _ = Executor.ExecuteAfterDelayAsync(TimeSpan.FromSeconds(3), () => m.DeleteAsync());
        }
    }
}
