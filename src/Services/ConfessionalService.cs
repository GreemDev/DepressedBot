using System;
using System.Threading.Tasks;
using DepressedBot.Commands;
using DepressedBot.Data;
using Discord;
using Discord.WebSocket;
using Gommon;

namespace DepressedBot.Services
{
    [Service("Confessional", "The main Service for handling church-style confessions and posting them to a channel.")]
    public class ConfessionalService
    {
        public async Task HandleMessageAsync(DepressedBotContext ctx)
        {
            var confessional = ctx.Client.GetGuild(385902350432206849)
                .GetTextChannel(726990911082594344);
            if (ctx.Message.Content.StartsWith($"{Config.CommandPrefix}confess"))
            {
                var confession = ctx.Message.Content.Replace($"{Config.CommandPrefix}confess", "",
                    StringComparison.OrdinalIgnoreCase);
                if (confession.IsNullOrEmpty())
                {
                    await ctx.Message.Channel.SendMessageAsync("You didn't confess anything.");
                    return;
                }
                if (ctx.Channel is null)
                {
                    var m = await confessional.SendMessageAsync(embed: new EmbedBuilder()
                        .WithAuthor(x =>
                        {
                            x.IconUrl = confessional.Guild.IconUrl;
                            x.Name = "DepressedBot Confessional";
                        })
                        .WithColor(Config.SuccessColor)
                        .WithDescription(confession)
                        .WithCurrentTimestamp()
                        .Build());
                    await ctx.Message.Channel.SendMessageAsync(embed: new EmbedBuilder()
                        .WithDescription($"[Confession Accepted]({m.GetJumpUrl()})").Build());
                }
                else
                {
                    await ctx.Message.Channel.SendMessageAsync("You must send that command in my DM so it stays classified.");
                }
            }
        }
    }
}