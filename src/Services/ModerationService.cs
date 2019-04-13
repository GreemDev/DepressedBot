using System.Threading.Tasks;
using DepressedBot.Data.Objects.EventArgs;

namespace DepressedBot.Services
{
    [Service("Moderation", "The main Service for handling moderation.")]
    public sealed class ModerationService
    {
        public async Task OnMessageReceivedAsync(MessageReceivedEventArgs args)
        {
            await Rules(args);
        }

        private async Task Rules(MessageReceivedEventArgs args)
        {
            if (args.Message.Channel.Id != 452962712318902303) return;
            if (args.Message.Author.Id != 168548441939509248 && args.Message.Author.Id != 385899728283500546) //hardcoded ids reeee
            {
                await args.Message.DeleteAsync();
            }
        }
    }
}
