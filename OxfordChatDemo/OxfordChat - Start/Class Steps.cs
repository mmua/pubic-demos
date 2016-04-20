// OxfordChatClient

            var responseBody = await baseUri
                .SetQueryParam("fromTimeStamp", fromTimeStamp)
                .GetStringAsync();

            var result = JsonConvert.DeserializeObject<List<Message>>(responseBody);
            return result;

// MainWindow.xaml.cs

        private OxfordChatClient _oxfordChatService = new OxfordChatClient();

// MainWindow.xaml.cs Timer_Elapsed

            var messages = await _oxfordChatService.GetMessagesAsync(_lastTimeStamp);
            await Dispatcher.InvokeAsync(() =>
            {
                foreach (var m in messages.OrderBy(m => m.Time))
                {
                    m.SendByMe = m.Sender == UserName;
                    Messages.Add(m);
                }
            });

            _lastTimeStamp = messages.Count() > 0 ? messages.Max(m => m.TimeStamp) : _lastTimeStamp;
            _timer.Start();

// MainWindow.xaml.cs Send_Click
            if (UserName == null || Text == null)
                return;

            var text = Text;
            Text = null;

            await _oxfordChatService.SendMessageAsync(UserName, text);



// SpellCkeckClient

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

// MainWindow.xaml.cs 

        private SpellCheckClient _spellCheckClient = new SpellCheckClient(@"af1f8dd1ccc442aaa7de91b102828843");

         //  Send_Click
            text = await _spellCheckClient.SpellAsync(text);

// MainWindow.xaml.cs
        private SpeechClient _speechClient = new SpeechClient(@"776604752d634792bfd0d82b1933e487", @"776604752d634792bfd0d82b1933e487");
            
            // ChatElement_MouseDown
            var text = (sender as Grid).Children.OfType<TextBlock>().Where(tb => (string)tb.Tag == "MessageText").FirstOrDefault()?.Text;
            var sound = await _speechClient.SynthesizeAsync(text);
            var play = new SoundPlayer(sound);
            play.Play();

 // MainWindow.xaml.cs
        private MicrophoneRecognitionClient micRecognitionClient = SpeechRecognitionServiceFactory.CreateMicrophoneClient(SpeechRecognitionMode.ShortPhrase, "en-us", @"776604752d634792bfd0d82b1933e487", @"378fa70ab6644baf8a63ac8b323ebb2d");
        
        // ctor
            micRecognitionClient.OnPartialResponseReceived += Record_PartialResponseReceived;
            micRecognitionClient.OnResponseReceived += Record_ResponseReceived;

        private void Record_Click(object sender, RoutedEventArgs e)
        {
            if (RecordButtonText == "Record")
            {
                micRecognitionClient.StartMicAndRecognition();
                RecordButtonText = "Stop";
            }
            else
            {
                micRecognitionClient.EndMicAndRecognition();
                RecordButtonText = "Record";
            }
        }

        private async void Record_PartialResponseReceived(object sender, PartialSpeechResponseEventArgs e)
        {
            await Dispatcher.InvokeAsync(() => Text = e?.PartialResult);
        }

        private async void Record_ResponseReceived(object sender, SpeechResponseEventArgs e)
        {
            micRecognitionClient.EndMicAndRecognition();
            await Dispatcher.InvokeAsync(() =>
            {
                Text = e?.PhraseResponse?.Results.FirstOrDefault()?.DisplayText;
                RecordButtonText = "Record";
            });
        }

// Oxford Chat Client
        private TextAnalyticsClient _textAnalyticsClient = new TextAnalyticsClient(@"400878dfced943b5baabd3f798110ddf");

        // SendMessageAsync
                Sentiment = await _textAnalyticsClient.GetSentimentAsync(text)