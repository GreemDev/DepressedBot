﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DepressedBot.Data;
using DepressedBot.Data.Objects.EventArgs;
using Discord;
using Gommon;

namespace DepressedBot.Services
{
    [Service("Dad", "The main Service for handling the DadBot-esque messages.")]
    public sealed class DadService
    {
        public async Task OnMessageReceivedAsync(MessageReceivedEventArgs args)
        {
            if (Config.IgnoredCategoryIds.Contains(args.Context.Channel.CategoryId ?? 0)) return;
            if (!ShouldReply(args.Message, out var resp)) return;
            var user = await args.Context.Guild.GetCurrentUserAsync();
            var m = await args.Context.ReplyAsync($"Hi {resp}, I'm {user.Nickname ?? user.Username}!");
            await Executor.ExecuteAfterDelayAsync(4000, async () => await m.DeleteAsync());
        }

        private bool ShouldReply(IMessage m, out string response)
        {
            if (m.Content.StartsWith("i'm ", StringComparison.OrdinalIgnoreCase))
            {
                response = m.Content.Substring(3).Trim();
                return true;
            }

            if (m.Content.StartsWith("im ", StringComparison.OrdinalIgnoreCase))
            {
                response = m.Content.Substring(3).Trim();
                return true;
            }

            response = string.Empty;
            return false;
        }
    }
}