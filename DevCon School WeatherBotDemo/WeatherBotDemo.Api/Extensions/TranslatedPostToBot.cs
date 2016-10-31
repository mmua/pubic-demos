using Microsoft.Bot.Builder.Dialogs.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeatherBotDemo.Services.BingTranslator;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;

namespace WeatherBotDemo.Api.Extensions
{
    public sealed class TranslatedPostToBot : IPostToBot
    {
        private readonly IPostToBot inner;
        private readonly ITranslator translator;
        public TranslatedPostToBot(IPostToBot inner, ITranslator translator)
        {
            SetField.NotNull(out this.inner, nameof(inner), inner);
            SetField.NotNull(out this.translator, nameof(translator), translator);
        }

        async Task IPostToBot.PostAsync<T>(T item, CancellationToken token)
        {
            var message = item as IMessageActivity;
            if (message != null)
            {
                await translator.TranslateMessage(message, "en-US");
            }
            await inner.PostAsync<T>(item, token);
        }
    }
}