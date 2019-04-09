﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DepressedBot.Extensions;
using Discord;
using Discord.WebSocket;
using Qmmands;

namespace DepressedBot.Commands.TypeParsers
{
    public sealed class ChannelParser<TChannel> : TypeParser<TChannel> where TChannel : SocketTextChannel
    {
        public override async Task<TypeParserResult<TChannel>> ParseAsync(
            Parameter param,
            string value,
            ICommandContext context,
            IServiceProvider provider)
        {
            var ctx = (DepressedBotContext)context;
            TChannel channel = null;

            if (ulong.TryParse(value, out var id) || MentionUtils.TryParseChannel(value, out id))
                channel = (await ctx.Guild.GetTextChannelsAsync()).FirstOrDefault(x => x.Id == id) as TChannel;

            if (channel is null)
            {
                var match = (await ctx.Guild.GetTextChannelsAsync()).Where(x => x.Name.EqualsIgnoreCase(value))
                    .ToList();
                if (match.Count > 1)
                    return TypeParserResult<TChannel>.Unsuccessful(
                        "Multiple channels found. Try mentioning the channel or using its ID.");
            }

            return channel is null
                ? TypeParserResult<TChannel>.Unsuccessful("Channel not found.")
                : TypeParserResult<TChannel>.Successful(channel);
        }
    }
}
