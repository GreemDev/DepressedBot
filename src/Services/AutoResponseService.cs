using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using DepressedBot.Data;
using DepressedBot.Data.Objects.EventArgs;
using DepressedBot.Extensions;

namespace DepressedBot.Services
{
    [Service("AutoResponse", "The main Service for handling AutoResponses.")]
    public sealed class AutoResponseService
    {
        public  Task OnMessageReceivedAsync(MessageReceivedEventArgs args)
        {
            foreach (var resp in AutoResponses.Responses)
            {
                if (args.Message.Content.EqualsIgnoreCase(resp.Phrase) && resp.Enabled)
                {
                    _ = Task.Run(async () =>
                    {
                        var m = await args.Context.ReplyAsync(resp.Response);
                        await Task.Delay(4000)
                            .ContinueWith(async _ => await m.DeleteAsync());
                    });
                    return Task.CompletedTask;
                }
            }

            return Task.CompletedTask;
        }
    }
}