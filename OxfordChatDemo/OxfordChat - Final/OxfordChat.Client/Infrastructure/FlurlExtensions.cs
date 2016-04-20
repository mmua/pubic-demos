using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OxfordChat.Client.Infrastructure
{
    public static class FlurlExtensions
    {
        public static Task<HttpResponseMessage> PostStringAsync(this FlurlClient client, string data, Encoding encoding, string mediaType)
        {
            var content = new StringContent(data, encoding, mediaType);
            return client.SendAsync(HttpMethod.Post, content: content);
        }
    }
}
