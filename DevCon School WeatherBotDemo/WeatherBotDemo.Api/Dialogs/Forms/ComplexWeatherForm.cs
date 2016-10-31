using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using WeatherBotDemo.Services.OpenWeatherMap;
using Microsoft.Bot.Connector;

namespace WeatherBotDemo.Api.Dialogs.Forms
{
    [Serializable]
    [Template(TemplateUsage.NotUnderstood, "I don't understand \"{0}\".", "Try again, I didn't get \"{0}\".")]
    public class ComplexWeatherForm
    {
        [Prompt("What {&} do you want?", "What {&}?")]
        public string City { get; set; }

        [Optional]
        public DateTime? Date { get; set; } = DateTime.Now.Date;

        [Prompt("What parameter would you like to know? {||}", ChoiceStyle = ChoiceStyleOptions.Inline)]
        public ComplexParameterOptions Parameter { get; set; }

        public static IForm<ComplexWeatherForm> BuildForm()
        {
            return new FormBuilder<ComplexWeatherForm>()
                .Field(nameof(City))
                .Field(nameof(Date), validate: (s, ov) =>
                {
                    var v = (DateTime?)ov;
                    var isValid = v != null && v >= DateTime.Now.Date && v <= DateTime.Now.Date.AddDays(5);
                    return Task.FromResult(new ValidateResult { IsValid = isValid, Value = v?.Date, Feedback = isValid ? null : "Only dates in a five days range from today is allowed" });
                })
                .AddRemainingFields()
                .OnCompletion(CompleteDialog)
                .Build();
        }

        private static async Task CompleteDialog(IDialogContext context, ComplexWeatherForm state)
        {
            await context.PostAsync("Wait a sec. Thinking...", "en-US");

            var weatherClient = new WeatherClient("88597cb7a556c191905de0f52f23d7d6");
            string message;
            try
            {
                var forecastArray = await weatherClient.Forecast(state.City);
                var forecast = forecastArray?.SingleOrDefault(f => f.When.Date == state.Date?.Date);

                if (forecast != null)
                {
                    if (state.Parameter == ComplexParameterOptions.Humidity) { message = $"The humidity on {forecast.ShortDate} in {forecast.City} is {forecast.Humidity}\r\n"; }
                    else if (state.Parameter == ComplexParameterOptions.Pressure) { message = $"The pressure on {forecast.ShortDate} in {forecast.City} is {forecast.Pressure}\r\n"; }
                    else if (state.Parameter == ComplexParameterOptions.Temperature) { message = $"The temperature on {forecast.ShortDate} in {forecast.City} is {forecast.Temp}\r\n"; }
                    else { message = "Sorry, unknown parameter \"{parameter}\" requested... Try again"; }
                }
                else { message = "Sorry! I was not able to get the forecast."; }
            }
            catch (Exception)
            {
                message = $"Sorry! I was not able to get the forecast.";
            }

            await context.PostAsync(message, "en-US");
            context.Done<object>(null);
        }
    }

    public enum ComplexParameterOptions
    {
        [Terms(@"Temp\w*", @"T")]
        Temperature = 1,
        [Terms(@"Pres\w*", @"P")]
        Pressure = 2,
        [Terms(@"Hum\w*", @"H")]
        Humidity = 3
    }
}