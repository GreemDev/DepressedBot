using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using DepressedBot.Extensions;
using Gommon;
using Humanizer;
using Qmmands;

namespace DepressedBot.Commands.Modules.Utility
{
    public partial class UtilityModule : DepressedBotModule
    {
        [Command("About")]
        [Description("Shows information about DepressedBot.")]
        [Usage("|prefix|about")]
        public async Task AboutAsync()
        {
            await Context.CreateEmbedBuilder()
                .AddField("Version", Version.FullVersion, true)
                .AddField("Author", "<@168548441939509248>", true)
                .AddField("Language", $"C#, Discord.Net {Version.DiscordNetVersion}", true)
                .AddField("Users", Context.Client.Guilds.SelectMany(x => x.Users).DistinctBy(x => x.Id).Count(x => !x.IsBot), true)
                .AddField("Channels", Context.Client.Guilds.SelectMany(x => x.Channels).DistinctBy(x => x.Id).Count(), true)
                .AddField(".NET Core Version", GetNetCoreVersion(out var ver) ? ver : ".NET Core version could not be retrieved.", true)
                .AddField("Host Operating System", Environment.OSVersion.Platform, true)
                .AddField("Uptime", (DateTime.Now - Process.GetCurrentProcess().StartTime).Humanize(3), true)
                .WithThumbnailUrl(Context.Client.CurrentUser.GetAvatarUrl())
                .SendToAsync(Context.Channel);
        }

        private bool GetNetCoreVersion(out string version)
        {
            Process process;
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                {
                    process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = "/bin/bash",
                            Arguments = "-c \"dotnet --version\"",
                            RedirectStandardError = true,
                            RedirectStandardOutput = true
                        }
                    };
                    process.Start();
                    version = process.StandardOutput.ReadToEnd();
                    return true;
                }

                case PlatformID.Win32NT:
                    process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            WindowStyle = ProcessWindowStyle.Hidden,
                            FileName = "cmd.exe",
                            Arguments = "/C dotnet --version",
                            RedirectStandardError = true,
                            RedirectStandardOutput = true
                        }
                    };
                    process.Start();
                    version = process.StandardOutput.ReadToEnd();
                    return true;
                default:
                    version = string.Empty;
                    return false;
            }
        }
    }
}
