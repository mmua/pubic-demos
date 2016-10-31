using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WeatherBotDemo.Services.BingTranslator;

namespace WeatherBotDemo.Api.Extensions
{
    public static class ITranslatorExtension
    {
        public static async Task TranslateMessage(this ITranslator translator, IMessageActivity message, string toLocale)
        {
            if (message == null || message.Locale == null || toLocale == null || message.Locale == toLocale)
                return;

            try
            {
                var translatedText = await translator.Translate(message.Text, message.Locale, toLocale);
                message.Text = translatedText;
                //message.Locale = toLocale;
                return;
            }
            catch (Exception e)
            {
                return;
            }
        }
    }
}