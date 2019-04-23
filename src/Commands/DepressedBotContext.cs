using System;
using System.Linq;
using System.Threading.Tasks;
using DepressedBot.Extensions;
using DepressedBot.Services;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;

namespace DepressedBot.Commands
{
    public sealed class DepressedBotContext : ICommandContext
    {
        private readonly EmojiService _emojiService;

        public DepressedBotContext(IDiscordClient client, IUserMessage msg, IServiceProvider provider)
        {
            _emojiService = provider.GetRequiredService<EmojiService>();
            Client = client as DiscordSocketClient;
            ServiceProvider = provider;
            Guild = (msg.Channel as ITextChannel)?.Guild;
            Channel = msg.Channel as ITextChannel;
            User = msg.Author as IGuildUser;
            Message = msg;
        }

        public DiscordSocketClient Client { get; }
        public IServiceProvider ServiceProvider { get; }
        public IGuild Guild { get; }
        public ITextChannel Channel { get; }
        public IGuildUser User { get; }
        public IUserMessage Message { get; }

        public Task ReactFailureAsync() => Message.AddReactionAsync(new Emoji(_emojiService.X));
        public Task ReactSuccessAsync() => Message.AddReactionAsync(new Emoji(_emojiService.BALLOT_BOX_WITH_CHECK));

        public Embed CreateEmbed(string content) => new EmbedBuilder().WithSuccessColor().WithAuthor(User)
            .WithDescription(content ?? string.Empty).Build();

        public EmbedBuilder CreateEmbedBuilder(string content = null) => CreateEmbed(content).ToEmbedBuilder();

        public Task<IUserMessage> ReplyAsync(string content) => Channel.SendMessageAsync(content);
        public Task<IUserMessage> ReplyAsync(Embed embed) => embed.SendToAsync(Channel);
        public Task<IUserMessage> ReplyAsync(EmbedBuilder embed) => embed.SendToAsync(Channel);
        public Task ReactAsync(string unicode) => Message.AddReactionAsync(new Emoji(unicode));

        public Task ReactAsync(ulong emoteId) => Message.AddReactionAsync(Guild.Emotes.FirstOrDefault(x => x.Id == emoteId));
    }
}
