﻿using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using DepressedBot.Data;
using DepressedBot.Extensions;
using Qmmands;

namespace DepressedBot.Commands.Modules.Help
{
    public partial class HelpModule : DepressedBotModule
    {
        [Command("Help", "H")]
        [Description("Shows the commands used for module listing, command listing, and command info.")]
        [Usage("|prefix|help")]
        public async Task HelpAsync()
        {
            await Context.CreateEmbedBuilder()
                .WithDescription("Hey, I'm DepressedBot! Here's a list of my commands designed to help you out! " +
                                 $"If you're new here, try out `{Config.CommandPrefix}mdls` to list all of my modules!" +
                                 "\n\n" +
                                 "{} = required argument | [] = optional argument" +
                                 "\n   \n" +
                                 $"**List Modules**: {Config.CommandPrefix}mdls" +
                                 "\n" +
                                 $"**List Commands in a Module**: {Config.CommandPrefix}cmds {{moduleName}}" +
                                 "\n" +
                                 $"**Show Info about a Command**: {Config.CommandPrefix}cmd {{commandName}}")
                .SendToAsync(Context.Channel);
        }
    }
}
