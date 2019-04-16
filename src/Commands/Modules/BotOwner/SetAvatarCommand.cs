using System;
using System.Net.Http;
using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using DepressedBot.Extensions;
using Discord;
using Gommon;
using Qmmands;

namespace DepressedBot.Commands.Modules.BotOwner
{
    public partial class BotOwnerModule : DepressedBotModule
    {
        public HttpClient Http { get; set; }

        [Command("SetAvatar")]
        [Description("Sets the avatar of the currently logged in account.")]
        [Usage("|prefix|setavatar {url}")]
        public async Task SetAvatarAsync(string url)
        {
            if (url.IsNullOrWhitespace() || !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                await Context.CreateEmbed("URL is malformed or empty.").SendToAsync(Context.Channel);
                return;
            }

            using (var sr = await Http.GetAsync(new Uri(url), HttpCompletionOption.ResponseHeadersRead))
            {
                if (!sr.IsImage())
                {
                    await Context.CreateEmbed("Provided URL does not lead to an image.")
                        .SendToAsync(Context.Channel);
                    return;
                }

                using (var img = (await sr.Content.ReadAsByteArrayAsync()).ToStream())
                {
                    await Context.Client.CurrentUser.ModifyAsync(x => x.Avatar = new Image(img));
                    await Context.CreateEmbedBuilder("Set this as my avatar.").WithImageUrl(url)
                        .SendToAsync(Context.Channel);
                }
            }
        }
    }
}
