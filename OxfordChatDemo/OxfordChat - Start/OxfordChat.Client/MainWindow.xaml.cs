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
        }

        private void MainWindows_Loaded(object sender, RoutedEventArgs e)
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

        }

        private async void Send_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void ChatElement_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void Record_Click(object sender, RoutedEventArgs e)
        {
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
    }
}
