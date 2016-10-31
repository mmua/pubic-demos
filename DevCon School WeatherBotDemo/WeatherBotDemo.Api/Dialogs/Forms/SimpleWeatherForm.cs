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
    public class SimpleWeatherForm
    {
        public string City { get; set; }
        public DateTime Date { get; set; }
        public ComplexParameterOptions Parameter { get; set; }

        public static IForm<SimpleWeatherForm> BuildForm()
        {
            return new FormBuilder<SimpleWeatherForm>()
                .Message("Welcome to the simple weather bot. Let's clarify some options")
                .OnCompletion(CompleteDialog)
                .Build();
        }

        private static async Task CompleteDialog(IDialogContext context, SimpleWeatherForm state)
        {
            var weatherClient = new WeatherClient("88597cb7a556c191905de0f52f23d7d6");
            string message;
            try
            {
                var forecastArray = await weatherClient.Forecast(state.City);
                var forecast = forecastArray?.SingleOrDefault(f => f.When.Date == state.Date.Date);

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

    public enum SimpleParameterOptions
    {
        Temperature = 1,
        Pressure = 2,
        Humidity = 3
    }
}