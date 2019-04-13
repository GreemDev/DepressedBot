using System;
using System.Linq;
using System.Threading.Tasks;
using Gommon;
using Qmmands;

namespace DepressedBot.Commands.Attributes
{
    public sealed class RequireAdminAttribute : CheckBaseAttribute
    {
        public override async Task<CheckResult> CheckAsync(ICommandContext context, IServiceProvider provider)
        {
            var ctx = context.Cast<DepressedBotContext>();
            if (ctx.User.RoleIds.Any(x => x == 385903172176183298))
            {
                return CheckResult.Successful;
            }

            await ctx.ReactFailureAsync();
            return CheckResult.Unsuccessful("Insufficient permission.");
        }
    }
}
