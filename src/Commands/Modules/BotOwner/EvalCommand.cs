using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using DepressedBot.Data.Objects;
using DepressedBot.Extensions;
using Discord;
using Gommon;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Qmmands;

namespace DepressedBot.Commands.Modules.BotOwner
{
    public partial class BotOwnerModule : DepressedBotModule
    {
        [Command("Eval", "Evaluate")]
        [Description("Evaluates C# code.")]
        [Usage("|prefix|eval [code]")]
        [RequireBotOwner]
        public Task EvalAsync([Remainder] string code = "")
        {
            _ = Executor.ExecuteAsync(async () =>
            {
                try
                {
                    var sopts = ScriptOptions.Default;
                    var embed = Context.CreateEmbedBuilder();
                    if (code.Contains("```cs"))
                    {
                        code = code.Remove(code.IndexOf("```cs", StringComparison.Ordinal), 5);
                        code = code.Remove(code.LastIndexOf("```", StringComparison.Ordinal), 3);
                    }

                    var objects = new EvalObjects
                    {
                        Context = Context,
                        Client = Context.Client,
                        Logger = Logger,
                        CommandService = CommandService,
                        EmojiService = EmojiService
                    };

                    if (code.Equals(""))
                    {
                        await embed.WithDescription(
                                "Context: [DepressedBot.Commands.DepressedBotContext](https://github.com/GreemDev/Volte/blob/rewrite/src/Commands/VolteContext.cs)\n" +
                                "Client: [Discord.DiscordSocketClient](https://docs.stillu.cc/api/Discord.WebSocket.DiscordSocketClient.html)\n" +
                                "Logger: [DepressedBot.Services.LoggingService](https://github.com/GreemDev/Volte/blob/rewrite/src/Services/LoggingService.cs)\n" +
                                "CommandService: [Qmmands.CommandService](https://github.com/Quahu/Qmmands/blob/master/src/Qmmands/CommandService.cs)\n" +
                                "EmojiService: [DepressedBot.Services.EmojiService](https://github.com/GreemDev/Volte/blob/rewrite/src/Services/EmojiService.cs)")
                            .SendToAsync(Context.Channel);
                        return;
                    }

                    var imports = new List<string>
                    {
                        "System", "System.Collections.Generic", "System.Linq", "System.Text",
                        "System.Diagnostics", "Discord", "Discord.WebSocket", "System.IO", "Gommon",
                        "System.Threading", "DepressedBot.Extensions", "DepressedBot.Data",
                        "DepressedBot.Discord", "DepressedBot.Services", "System.Threading.Tasks", "Qmmands"
                    };

                    sopts = sopts.WithImports(imports).WithReferences(AppDomain.CurrentDomain.GetAssemblies()
                        .Where(x => !x.IsDynamic && !x.Location.IsNullOrWhitespace()));


                    var msg = await embed.WithTitle("Evaluating...").SendToAsync(Context.Channel);
                    try
                    {
                        var sw = Stopwatch.StartNew();
                        var res = await CSharpScript.EvaluateAsync(code, sopts, objects);
                        sw.Stop();
                        if (res != null)
                        {
                            await msg.ModifyAsync(m =>
                                m.Embed = embed.WithTitle("Eval")
                                    .AddField("Elapsed Time", $"{sw.ElapsedMilliseconds}ms", true)
                                    .AddField("Return Type", res.GetType().FullName, true)
                                    .AddField("Output", Format.Code(res.ToString(), "css")).Build());
                        }
                        else
                        {
                            await msg.DeleteAsync();
                            await Context.ReactSuccessAsync();
                        }
                    }
                    catch (Exception e)
                    {
                        await msg.ModifyAsync(m =>
                            m.Embed = embed
                                .WithDescription($"`{e.Message}`")
                                .WithTitle("Error")
                                .Build()
                        );
                        File.WriteAllText("storage/EvalError.log", $"{e.Message}\n{e.StackTrace}");
                        await Context.Channel.SendFileAsync("storage/EvalError.log", string.Empty);
                        File.Delete("data/EvalError.log");
                    }
                    finally
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                    }
                }
                catch (Exception e)
                {
                    await Logger.Log(LogSeverity.Error, LogSource.Module, string.Empty, e);
                }
            });
            return Task.CompletedTask;
        }
    }
}