using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace OxfordChat.Client.Services
{
    public class SpellCheckClient
    {
        private string _baseUri = @"https://bingapis.azure-api.net/api/v5/spellcheck";
        private string _subscriptionKey;

        public SpellCheckClient(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
        }

        public async Task<string> SpellAsync(string text)
        {
            var response = await _baseUri
                .SetQueryParam("mode", "proof")
                .WithHeader("Ocp-Apim-Subscription-Key", _subscriptionKey)
                .PostUrlEncodedAsync(new { Text = text })
                .ReceiveJson();

            foreach(var t in response?.flaggedTokens)
            {
                try
                {
                    text = text.Replace(t.token, t.suggestions[0].suggestion);
                }
                catch(Exception)
                { }
            }

            return text;
        }
    }
}
