using OxfordChat.Client.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using Microsoft.ProjectOxford.SpeechRecognition;
using System.Media;

namespace OxfordChat.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private OxfordChatClient _oxfordChatService = new OxfordChatClient();
        private SpellCheckClient _spellCheckClient = new SpellCheckClient(@"3d00390fed99471eb7c32adafe5b8830");
        private MicrophoneRecognitionClient micRecognitionClient = SpeechRecognitionServiceFactory.CreateMicrophoneClient(SpeechRecognitionMode.ShortPhrase, "en-us", @"41b45bdcc78c42c4a297279084d2b217", @"41b45bdcc78c42c4a297279084d2b217");
        private SpeechClient _speechClient = new SpeechClient(@"41b45bdcc78c42c4a297279084d2b217", @"807f25315734429087d4507f7ebf25f7");

        private long? _lastTimeStamp;
        private System.Timers.Timer _timer;

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set { Set(nameof(UserName), ref _userName, value); }
        }

        private ObservableCollection<Message> _messages = new ObservableCollection<Message>();
        public ObservableCollection<Message> Messages
        {
            get { return _messages; }
            set { Set(nameof(Messages), ref _messages, value); }
        }

        private string _text;
        public string Text
        {
            get { return _text; }
            set { Set(nameof(Text), ref _text, value); }
        }

        private string _recordButtonText = "Record";
        public string RecordButtonText
        {
            get { return _recordButtonText; }
            set { Set(nameof(RecordButtonText), ref _recordButtonText, value); }
        }


        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindows_Loaded;

            micRecognitionClient.OnPartialResponseReceived += Record_PartialResponseReceived;
            micRecognitionClient.OnResponseReceived += Record_ResponseReceived;
        }

        private async void MainWindows_Loaded(object sender, RoutedEventArgs e)
        {
            UserName = "Guest" + (new Random()).Next(100).ToString();

            _timer = new System.Timers.Timer()
            {
                Interval = 500,
                AutoReset = false
            };
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
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
        }

        private async void Send_Click(object sender, RoutedEventArgs e)
        {
            if (UserName == null || Text == null)
                return;

            var text = Text;
            Text = null;

            text = await _spellCheckClient.SpellAsync(text);
            await _oxfordChatService.SendMessageAsync(UserName, text);
        }

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

        #region Infrastructure
        public event PropertyChangedEventHandler PropertyChanged;
        private void Set<T>(string propertyName, ref T field, T newValue)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        private void ScrollViewer_LayoutUpdated(object sender, EventArgs e)
        {
            if (ScrollViewer.VerticalOffset == ScrollViewer.ScrollableHeight)
            {
                ScrollViewer.ScrollToEnd();
            }
        }

        private async void ChatElement_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var text = (sender as Grid).Children.OfType<TextBlock>().Where(tb => (string)tb.Tag == "MessageText").FirstOrDefault()?.Text;
            var sound = await _speechClient.SynthesizeAsync(text);
            var play = new SoundPlayer(sound);
            play.Play();
        }
    }
}
