using Gommon;
using System.Linq;
using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using DepressedBot.Extensions;
using Qmmands;

namespace DepressedBot.Commands.Modules.Help
{
    public partial class HelpModule : DepressedBotModule
    {
        [Command("Modules", "Mdls")]
        [Description("Lists available modules.")]
        [Usage("|prefix|modules")]
        public async Task ModulesAsync()
        {
            await Context.CreateEmbedBuilder(
                $"`{CommandService.GetAllModules().Select(x => x.SanitizeModuleName()).Join("`, `")}`"
                ).WithTitle("Available Modules").SendToAsync(Context.Channel);
        }
    }
}
