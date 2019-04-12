﻿using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using DepressedBot.Extensions;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Qmmands;
using RestSharp;

namespace DepressedBot.Commands.Modules.Shitpost
{
    public partial class ShitpostModule : DepressedBotModule
    {
        [Command("Insult")]
        [Description("Insults the given user, or the command invocator if no user is given.")]
        [Usage("|prefix|insult [user]")]
        public async Task InsultAsync(SocketGuildUser user = null)
        {
            var target = user ?? Context.User;
            var http = new RestClient("https://insult.mattbas.org/api/insult.json");
            var insult = JsonConvert.DeserializeObject(http.Execute(new RestRequest()).Content).Cast<JObject>()
                .GetValue("insult");
            await Context.ReplyAsync($"{target.Mention}, {insult}");
        }
    }
}
