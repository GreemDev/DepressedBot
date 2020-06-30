using System;
using System.Threading.Tasks;
using DepressedBot.Data;
using Discord;
using Discord.WebSocket;

namespace DepressedBot.Services
{
    [Service("Confessional", "The main Service for handling church-style confessions and posting them to a channel.")]
    public class ConfessionalService
    {
        public async Task HandleMessageAsync(SocketUserMessage message)
        {
            var confessional = Discord.DepressedBot.Client.GetGuild(385902350432206849)
                .GetTextChannel(726990911082594344);
            if (message.Content.StartsWith($"{Config.CommandPrefix}confess"))
            {
                var confession = message.Content.Replace($"{Config.CommandPrefix}confess", "",
                    StringComparison.OrdinalIgnoreCase);
                if (message.Channel is IDMChannel)
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
                    await message.Channel.SendMessageAsync(embed: new EmbedBuilder()
                        .WithDescription($"[Confession Accepted]({m.GetJumpUrl()})").Build());
                }
                else
                {
                    await message.Channel.SendMessageAsync("You must send that command in my DM so it stays classified.");
                }
            }
        }
    }
}