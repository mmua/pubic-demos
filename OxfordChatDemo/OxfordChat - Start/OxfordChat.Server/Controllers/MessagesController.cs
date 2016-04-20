using OxfordChat.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OxfordChat.Server.Controllers
{
    public class MessagesController : ApiController
    {
        private static readonly List<Message> _messageStore;
        private static long _curTimeStamp = 1;

        static MessagesController()
        {
            _messageStore = new List<Message>();
            Initialize();
        }

        private static void Initialize()
        {
            _messageStore.Add(
                new Message
                {
                    Sender = "Admin",
                    Time = DateTime.Now,
                    Text = "Wellcome to our great chat!",
                    TimeStamp = _curTimeStamp++
                });
        }

        public IHttpActionResult Get([FromUri] long? fromTimeStamp = 0)
        {
            return Ok(_messageStore.Where(m => m.TimeStamp > fromTimeStamp).Take(50).OrderBy(m => m.Time));
        }

        public IHttpActionResult Post([FromBody] Message message)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            message.TimeStamp = _curTimeStamp++;
            _messageStore.Add(message);

            return Ok();
        }

        public IHttpActionResult Delete()
        {
            _messageStore.Clear();
            Initialize();

            return Ok();
        }
    }
}
