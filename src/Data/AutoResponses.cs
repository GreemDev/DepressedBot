using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DepressedBot.Data.Objects;
using Newtonsoft.Json;
using DepressedBot.Extensions;
using DepressedBot.Services;
using Discord;

namespace DepressedBot.Data
{
    public sealed class AutoResponses
    {
        private const string AutoResponseFile = "storage/autoresponses.json";
        public static List<AutoResponse> Responses;
        private static readonly bool _valid = File.Exists(AutoResponseFile) && !File.ReadAllText(AutoResponseFile).IsNullOrEmpty();

        static AutoResponses()
        {
            _ = CreateIfNotExists();
            if (_valid)
                Responses = JsonConvert.DeserializeObject<List<AutoResponse>>(File.ReadAllText(AutoResponseFile));
        }

        public static async Task CreateIfNotExists()
        {
            if (_valid) return;
            var logger = Discord.DepressedBot.GetService<LoggingService>();
            await logger.Log(LogSeverity.Warning, LogSource.DepressedBot,
                "autoresponses.json didn't exist or was empty. Created it for you.");
            Responses = new List<AutoResponse>
            {
                new AutoResponse()
                {
                    Phrase = "Example",
                    Response = "Example LOL",
                    Enabled = true
                }
            };
            File.WriteAllText(AutoResponseFile,
                JsonConvert.SerializeObject(Responses, Formatting.Indented));
        }

        public struct AutoResponse
        {
            public string Phrase { get; set; }
            public string Response { get; set; }
            public bool Enabled { get; set; }
        }
    }
}