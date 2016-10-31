using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using WeatherBotDemo.Services.OpenWeatherMap;
using System.Threading;

namespace WeatherBotDemo.Simple.Api
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        private WeatherClient _weatherClient = new WeatherClient("88597cb7a556c191905de0f52f23d7d6");

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                //var thinking = activity.CreateReply("Just a sec. Thinking...");
                //await connector.Conversations.ReplyToActivityAsync(thinking);

                //var typing = activity.CreateReply();
                //typing.Type = ActivityTypes.Typing;
                //await connector.Conversations.ReplyToActivityAsync(typing);
                //await Task.Delay(1000);

                try
                {
                    var prediction = await _weatherClient.Forecast(activity.Text);
                    var reply = activity.CreateReply($"Here is your forecast for {prediction?[0].City}:  \n {String.Join("  \n", prediction.Select(p => p.ToString()))}");
                    await connector.Conversations.ReplyToActivityAsync(reply);
                }
                catch (Exception)
                {
                    var error = activity.CreateReply($"Cannot find forecast for \"{activity.Text ?? "NULL"}\"");
                    await connector.Conversations.ReplyToActivityAsync(error);
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}