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
            string m;
            switch (DateTime.Now.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    m = "it's fucking suday. god.";
                    break;
                case DayOfWeek.Monday:
                    m = "it's fucking moday. god.";
                    break;
                case DayOfWeek.Tuesday:
                    m = "it's fucking tueaday. god.";
                    break;
                case DayOfWeek.Wednesday:
                    m = "it's fucking weaday. god.";
                    break;
                case DayOfWeek.Thursday:
                    m = "it's fucking thuday. god.";
                    break;
                case DayOfWeek.Friday:
                    m = "it's fucking friaday. god.";
                    break;
                case DayOfWeek.Saturday:
                    m = "it's fucking satuaday. god.";
                    break;
                default:
                    m = "apparently it's the eighth day of the week. god.";
                    break;
            }

            await Context.ReplyAsync(m);
        }
    }
}
