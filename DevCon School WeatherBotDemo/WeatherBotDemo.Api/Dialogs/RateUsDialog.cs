using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace WeatherBotDemo.Api.Dialogs
{
    [Serializable]
    public class RateUsDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            //await context.Wait<string>(ResumeMessage);
            PromptDialog.Number(context, ProcessRate, "Please rate your experience (from 1 to 5) with this bot, where 1 is the lowest.", "Didn't get that, please retry", 3);
        }

        private async Task ProcessRate(IDialogContext context, IAwaitable<long> result)
        {
            var rating = await result;
            if (rating >= 1 && rating <= 5)
            {
                var rateCount = 0;
                var avgRating = 0.0;
                context.UserData.TryGetValue(nameof(rateCount), out rateCount);
                context.UserData.TryGetValue(nameof(avgRating), out avgRating);
                avgRating = (avgRating * rateCount + rating) / (rateCount + 1);

                context.UserData.SetValue(nameof(rateCount), rateCount + 1);
                context.UserData.SetValue(nameof(avgRating), avgRating);

                await context.PostAsync($"Thanks! Your average rating is {avgRating}.", "en-US");
            }
            else
            {
                await context.PostAsync($"\"{rating}\" is not a number between 1 and 5. Never mind, lets do it next time.", "en-US");
            }

            context.Done<object>(null);
        }
    }
}