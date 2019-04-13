using Gommon;
using System.IO;
using System.Threading.Tasks;
using DepressedBot.Data.Objects;
using DepressedBot.Services;
using Discord;
using Newtonsoft.Json;

namespace DepressedBot.Data
{
    public sealed class Config
    {
        private const string ConfigFile = "storage/config.json";
        private static BotConfig _bot;
        private static readonly bool _valid = File.Exists(ConfigFile) && !File.ReadAllText(ConfigFile).IsNullOrEmpty();

        static Config()
        {
            _ = CreateIfNotExists();
            if (_valid)
                _bot = JsonConvert.DeserializeObject<BotConfig>(File.ReadAllText(ConfigFile));
        }

        public static async Task CreateIfNotExists()
        {
            if (_valid) return;
            var logger = Discord.DepressedBot.GetService<LoggingService>();
            await logger.Log(LogSeverity.Warning, LogSource.DepressedBot,
                "config.json didn't exist or was empty. Created it for you.");
            _bot = new BotConfig
            {
                Token = "token here",
                CommandPrefix = "$",
                Owner = 0,
                Game = "in DepressedBot V2 Code!",
                Streamer = "streamer here",
                SuccessEmbedColor = 0x7000FB,
                ErrorEmbedColor = 0xFF0000,
                LogAllCommands = true,
                IgnoredCategoryIds = new ulong[] {}
            };
            File.WriteAllText(ConfigFile,
                JsonConvert.SerializeObject(_bot, Formatting.Indented));
        }

        public static string Token => _bot.Token;

        public static string CommandPrefix => _bot.CommandPrefix;

        public static ulong Owner => _bot.Owner;

        public static string Game => _bot.Game;

        public static string Streamer => _bot.Streamer;

        public static uint SuccessColor => _bot.SuccessEmbedColor;

        public static uint ErrorColor => _bot.ErrorEmbedColor;

        public static bool LogAllCommands => _bot.LogAllCommands;

        public static ulong[] IgnoredCategoryIds => _bot.IgnoredCategoryIds;


        private struct BotConfig
        {
            public string Token;
            public string CommandPrefix;
            public ulong Owner;
            public string Game;
            public string Streamer;
            public uint SuccessEmbedColor;
            public uint ErrorEmbedColor;
            public bool LogAllCommands;
            public ulong[] IgnoredCategoryIds;
        }
    }
}
