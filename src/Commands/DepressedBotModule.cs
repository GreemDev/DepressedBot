using DepressedBot.Services;
using Qmmands;

namespace DepressedBot.Commands
{
    public abstract class DepressedBotModule : ModuleBase<DepressedBotContext>
    {
        public CommandService CommandService { get; set; }
        public EmojiService EmojiService { get; set; }
        public LoggingService Logger { get; set; }
    }
}
