using System;
using System.Threading.Tasks;
using DepressedBot.Data;
using DepressedBot.Extensions;
using Qmmands;

namespace DepressedBot.Commands.Attributes
{
    public sealed class RequireBotOwnerAttribute : CheckBaseAttribute
    {
        public override async Task<CheckResult> CheckAsync(ICommandContext context, IServiceProvider provider)
        {
            var ctx = context.Cast<DepressedBotContext>();
            if (ctx.User.Id == Config.Owner) return CheckResult.Successful;
            await ctx.ReactFailureAsync();
            return CheckResult.Unsuccessful("Insufficient permission.");
        }
    }
}
