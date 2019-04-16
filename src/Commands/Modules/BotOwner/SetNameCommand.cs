using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using DepressedBot.Extensions;
using Qmmands;

namespace DepressedBot.Commands.Modules.BotOwner
{
    public partial class BotOwnerModule : DepressedBotModule
    {
        [Command("SetName")]
        [Description("Sets the username of the currently logged in account.")]
        [Usage("|prefix|setname {name}")]
        public async Task SetNameAsync([Remainder]string name)
        {
            await Context.Client.CurrentUser.ModifyAsync(x => x.Username = name);
            await Context.CreateEmbed($"Set my username to **{name}**!").SendToAsync(Context.Channel);
        }
    }
}
