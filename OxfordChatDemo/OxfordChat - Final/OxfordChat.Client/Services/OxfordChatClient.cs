using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Flurl;
using Flurl.Http;
using System.Globalization;

namespace OxfordChat.Client.Services
{
    public class OxfordChatClient
    {
        private string baseUri =  @"http://oc-api.azurewebsites.net/api/Messages";
        private TextAnalyticsClient _textAnalyticsClient = new TextAnalyticsClient(@"2d352d1003a24c989c985f01b8601f53");

        public async Task<IEnumerable<Message>> GetMessagesAsync(long? fromTimeStamp = null)
        {
            var responseBody = await baseUri
                .SetQueryParam("fromTimeStamp", fromTimeStamp)
                .GetStringAsync();

            var result = JsonConvert.DeserializeObject<List<Message>>(responseBody);
            return result;
        }

        public async Task SendMessageAsync(string sender, string text)
        {
            var message = new Message()
            {
                Time = DateTimeOffset.Now,
                Sender = sender,
                Text = text,
                Sentiment = await _textAnalyticsClient.GetSentimentAsync(text)
            };

            await baseUri.PostJsonAsync(message);
        }
    }
}
