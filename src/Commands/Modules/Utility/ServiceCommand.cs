using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using DepressedBot.Extensions;
using DepressedBot.Services;
using Gommon;
using Qmmands;
using RestSharp.Extensions;

namespace DepressedBot.Commands.Modules.Utility
{
    public partial class UtilityModule : DepressedBotModule
    {
        [Command("Service")]
        [Description("Gets an internal DepressedBot Service as well as its purpose.")]
        [Usage("|prefix|service {name}")]
        public async Task ServiceAsync([Remainder] string name = "")
        {
            var services = Assembly.GetEntryAssembly()?.GetTypes()
                .Where(x => x.HasAttribute<ServiceAttribute>()).ToArray();
            var type = services?.FirstOrDefault(x => x.Name.ContainsIgnoreCase(name));
            if (type is null)
            {
                await Context.CreateEmbed($"{EmojiService.X} Service not found.").SendToAsync(Context.Channel);
                return;
            }

            if (name == "")
            {
                await Context.CreateEmbedBuilder(services.Select(x => $"**{x.Name}**").Join("\n"))
                    .WithTitle("Available Services").SendToAsync(Context.Channel);
                return;
            }

            var target = type.GetAttribute<ServiceAttribute>();

            await Context.CreateEmbedBuilder()
                .AddField("Name", target.Name, true)
                .AddField("Purpose", target.Purpose, true)
                .SendToAsync(Context.Channel);
        }
    }
}
