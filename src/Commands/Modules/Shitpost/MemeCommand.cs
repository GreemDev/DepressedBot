using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using DepressedBot.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Qmmands;
using RestSharp;

namespace DepressedBot.Commands.Modules.Shitpost
{
    public partial class ShitpostModule : DepressedBotModule
    {
        [Command("Meme")]
        [Description("Gets a random meme from https://some-random-api.ml/meme. Memes may or may not be good.")]
        [Usage("|prefix|meme")]
        public async Task MemeAsync()
        {
            var http = new RestClient("https://some-random-api.ml/meme");
            var resp = JsonConvert.DeserializeObject<JObject>(http.Execute(new RestRequest()).Content);
            await Context.CreateEmbedBuilder().WithTitle($"{resp.GetValue("caption")}")
                .WithImageUrl($"{resp.GetValue("image")}").SendToAsync(Context.Channel);

        }
    }
}
