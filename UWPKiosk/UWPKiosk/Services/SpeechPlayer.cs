using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace UWPKiosk.Services
{
    class SpeechPlayer : ISpeechPlayer
    {
        public async Task PlayAsync(Stream speechStream, string contentFormat)
        {
            if (speechStream == null) throw new ArgumentNullException(nameof(speechStream));
            if (contentFormat == null) throw new ArgumentNullException(nameof(speechStream));

            var media = new MediaElement();
            media.SetSource(speechStream.AsRandomAccessStream(), contentFormat);
            media.Play();

            await Task.CompletedTask;
        }
    }
}
