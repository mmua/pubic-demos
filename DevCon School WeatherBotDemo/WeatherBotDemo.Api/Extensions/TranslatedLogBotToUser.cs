using Microsoft.Bot.Builder.Dialogs.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Connector;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Internals.Fibers;
using WeatherBotDemo.Services.BingTranslator;

namespace WeatherBotDemo.Api.Extensions
{
    public class TranslatedBotToUser : IBotToUser
    {
        private readonly IBotToUser inner;
        private readonly ITranslator translator;

        public TranslatedBotToUser(IBotToUser inner, ITranslator translator)
        {
            SetField.NotNull(out this.inner, nameof(inner), inner);
            SetField.NotNull(out this.translator, nameof(translator), translator);
        }

        IMessageActivity IBotToUser.MakeMessage()
        {
            var message = this.inner.MakeMessage();
            message.Locale = "en-US"; // durty hack!
            return message;
        }

        async Task IBotToUser.PostAsync(IMessageActivity message, CancellationToken cancellationToken)
        {
            await translator.TranslateMessage(message, Thread.CurrentThread?.CurrentCulture?.Name);
            await this.inner.PostAsync(message, cancellationToken);
        }
    }
}