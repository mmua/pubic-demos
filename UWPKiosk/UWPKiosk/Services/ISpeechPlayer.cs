using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace UWPKiosk.Services
{
    public interface ISpeechPlayer
    {
        Task PlayAsync(Stream speechStream, string contentType);
    }
}
