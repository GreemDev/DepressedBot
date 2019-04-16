using System.Linq;
using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using DepressedBot.Extensions;
using Gommon;
using Qmmands;

namespace DepressedBot.Commands.Modules.Help
{
    public partial class HelpModule : DepressedBotModule
    {
        [Command("Commands", "Cmds")]
        [Description("Shows commands for a module.")]
        [Usage("|prefix|commands {module}")]
        public async Task CommandsAsync(string module)
        {
            var target = CommandService.GetAllModules().FirstOrDefault(m => m.SanitizeModuleName().EqualsIgnoreCase(module));
            if (target is null)
            {
                await Context.CreateEmbed($"{EmojiService.X} Specified module not found.").SendToAsync(Context.Channel);
                return;
            }

            if (CanShowModuleInfo(target))
            {
                await Context.CreateEmbed($"{EmojiService.X} You don't have permission to use the module that command is from.")
                    .SendToAsync(Context.Channel);
                return;
            }

            var commands = $"`{target.Commands.Select(x => x.FullAliases.First()).Join("`, `")}`";
            await Context.CreateEmbedBuilder(commands).WithTitle($"Commands for {target.SanitizeModuleName()}")
                .SendToAsync(Context.Channel);
        }

        private bool CanShowModuleInfo(Module m)
        {
            var owner = m.SanitizeModuleName().EqualsIgnoreCase("botowner") && !Context.User.IsBotOwner();
            return owner;
        }
    }
}
