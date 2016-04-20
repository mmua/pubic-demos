using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;

namespace OxfordChat.Client.Services
{
    class TextAnalyticsClient
    {
        private string _baseUrl = @"https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";
        private string _subscriptionKey;

        public TextAnalyticsClient(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
        }

        public async Task<double?> GetSentimentAsync(string text)
        {
            var content = new { documents = new List<object>() { new { id = "1", text = text } } };

            try
            {
                var response = await _baseUrl
                    .WithHeader("Ocp-Apim-Subscription-Key", _subscriptionKey)
                    .PostJsonAsync(content)
                    .ReceiveJson();

                return (double)response?.documents?[0]?.score;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
