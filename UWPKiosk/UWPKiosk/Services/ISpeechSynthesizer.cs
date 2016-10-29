using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.Storage.Streams;

namespace UWPKiosk.Services
{
    public interface ISpeechSynthesizer
    {
        string ContentType { get; }
        Task<Stream> SythesizeAsync(string ssmlFragment, string language = "en-US", Gender gender = Gender.Female);
    }

    public enum Gender
    {
        Male,
        Female
    }
}
