using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxfordChat.Client
{
    public class Message
    {
        public string Sender { get; set; }
        public DateTimeOffset Time { get; set; }
        public string Text { get; set; }
        public long TimeStamp { get; set; }
        public double? Sentiment { get; set; }

        [JsonIgnore]
        public bool SendByMe { get; set; }
    }
}
