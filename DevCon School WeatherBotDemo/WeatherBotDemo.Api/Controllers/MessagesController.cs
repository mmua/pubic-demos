using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using WeatherBotDemo.Api.Dialogs.Forms;
using WeatherBotDemo.Api.Dialogs;
using System.Text.RegularExpressions;

namespace WeatherBotDemo.Api
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                if (await ApplyUserLanguage(activity))
                    await Conversation.SendAsync(activity, MakeRootDialog);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private const string LanguageCommandPattern = @"^\s*[/\\]lang\s+(?<locale>\w{2}(-\w{2})?)\s*$";
        private async Task<bool> ApplyUserLanguage(IMessageActivity message)
        {
            var botState = new StateClient(new Uri(message.ServiceUrl)).BotState;
            var userData = await botState.GetUserDataAsync(message.ChannelId, message.From.Id);

            var match = Regex.Match(message.Text, LanguageCommandPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.ExplicitCapture | RegexOptions.Singleline);
            if (match.Success)
            {
                var locale = match.Groups["locale"].Value;
                userData.SetProperty("userLocale", locale);
                await botState.SetUserDataAsync(message.ChannelId, message.From.Id, userData);
                return false;
            }

            var savedLocale = userData.GetProperty<string>("userLocale");
            if (savedLocale != null)
                message.Locale = savedLocale;

            return true;
        }

        internal static IDialog<object> MakeRootDialog()
        {
            //return Chain.From(() => FormDialog.FromForm(ComplexWeatherForm.BuildForm))
            //    .Do((c, r) => c.PostAsync("Thank you for using Simple Weather Bot!", "en-US"))
            //    .ContinueWith<ComplexWeatherForm, object>((c, r) => Task.FromResult<IDialog<object>>(new RateUsDialog()));

            return Chain.From(() => new WeatherLuisDialog());
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