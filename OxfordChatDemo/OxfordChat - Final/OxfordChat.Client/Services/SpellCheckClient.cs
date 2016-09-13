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
        private string _baseUri = @"https://api.cognitive.microsoft.com/bing/v5.0/spellcheck/";
        private string _subscriptionKey;

        public SpellCheckClient(string subscriptionKey)
        {
            _subscriptionKey = subscriptionKey;
        }

        public async Task<string> SpellAsync(string text)
        {
            var response = await _baseUri
                .SetQueryParam("mode", "proof")
                .SetQueryParam("text", text)
                .WithHeader("Ocp-Apim-Subscription-Key", _subscriptionKey)
                .GetJsonAsync();

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
