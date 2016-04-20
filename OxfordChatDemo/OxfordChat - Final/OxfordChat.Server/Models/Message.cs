using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OxfordChat.Server.Models
{
    public class Message
    {
        [Required]
        public string Sender { get; set; }
        [Required]
        public DateTimeOffset? Time { get; set; }
        public string Text { get; set; }

        public double? Sentiment { get; set; }
        public long TimeStamp { get; set; }
    }
}