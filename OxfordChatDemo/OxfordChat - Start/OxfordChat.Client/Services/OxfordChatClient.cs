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
        private string baseUri = @"http://oxfordchatapi.azurewebsites.net/api/Messages";

        public async Task<IEnumerable<Message>> GetMessagesAsync(long? fromTimeStamp = null)
        {
            return null;
        }

        public async Task SendMessageAsync(string sender, string text)
        {
            var message = new Message()
            {
                Time = DateTimeOffset.Now,
                Sender = sender,
                Text = text
            };

            await baseUri.PostJsonAsync(message);
        }
    }
}
