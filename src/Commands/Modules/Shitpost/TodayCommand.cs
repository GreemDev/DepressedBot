using System;
using System.Threading.Tasks;
using DepressedBot.Commands.Attributes;
using Qmmands;

namespace DepressedBot.Commands.Modules.Shitpost
{
    public partial class ShitpostModule : DepressedBotModule
    {
        [Command("Today")]
        [Description("Shows the current day in the form of dog memes.")]
        [Usage("|prefix|today")]
        public async Task TodayAsync()
        {
            string m = DateTime.Now.DayOfWeek switch
            {
                DayOfWeek.Sunday => "it's fucking suday. god.",
                DayOfWeek.Monday => "it's fucking moday. god.",
                DayOfWeek.Tuesday => "it's fucking tueaday. god.",
                DayOfWeek.Wednesday => "it's fucking weaday. god.",
                DayOfWeek.Thursday => "it's fucking thuday. god.",
                DayOfWeek.Friday => "it's fucking friaday. god.",
                DayOfWeek.Saturday => "it's fucking satuaday. god.",
                _ => "apparently it's the eighth day of the week. god."
            };

            await Context.ReplyAsync(m);
        }
    }
}
