using System.Threading.Tasks;
using DepressedBot.Data.Objects.EventArgs;
using Discord;
using Gommon;

namespace DepressedBot.Services
{
    [Service("Moderation", "The main Service for handling moderation.")]
    public sealed class ModerationService
    {
        public async Task OnMessageReceivedAsync(MessageReceivedEventArgs args)
        {
            await CheckRulesChannelAsync(args);
            await CheckMusicChannelsAsync(args);
        }

        private async Task CheckRulesChannelAsync(MessageReceivedEventArgs args)
        {
            if (args.Message.Channel.Id != 452962712318902303) return;
            if (args.Message.Author.Id != 168548441939509248 && args.Message.Author.Id != 385899728283500546) //hardcoded ids reeee
            {
                await args.Message.DeleteAsync();
            }

        }

        public async Task CheckMusicChannelsAsync(MessageReceivedEventArgs args)
        {
            if (args.Context.Channel.CategoryId.GetValueOrDefault() != 542855804727066636) return;
            if (args.Context.Channel.Topic.ContainsIgnoreCase("whitelist-off")) return;
            if (!args.Context.Channel.Topic.Contains($"{args.Context.User.Id}"))
            {
                await args.Message.DeleteAsync(new RequestOptions
                {
                    AuditLogReason = "Automatically deleted because the user in question's ID was not in the channel topic."
                });
            }
        }
    }
}
