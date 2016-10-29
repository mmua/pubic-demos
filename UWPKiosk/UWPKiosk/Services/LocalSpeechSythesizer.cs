using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.Storage.Streams;
using Windows.Web.Http;

namespace UWPKiosk.Services
{
    class LocalSpeechSythesizer : ISpeechSynthesizer
    {
        private const string SsmlTemplate = @"<speak version='1.0' xmlns='http://www.w3.org/2001/10/synthesis' xml:lang='{0}'>{1}</speak>";

        public string ContentType => "audio/wav";

        public async Task<Stream> SythesizeAsync(string ssmlFragment, string language = "en-US", Gender gender = Gender.Female)
        {
            if (language == null) throw new ArgumentNullException(nameof(language));

            var voice = SpeechSynthesizer.AllVoices
                        .Where(v => v.Language == language && v.Gender == (VoiceGender)Enum.Parse(typeof(VoiceGender), gender.ToString(), true))
                        .FirstOrDefault();
            if (voice == null || ssmlFragment == null)
                return null;

            try
            {
                using (var synth = new SpeechSynthesizer())
                {
                    synth.Voice = voice;
                    var ssml = String.Format(SsmlTemplate, language, ssmlFragment);
                    return (await synth.SynthesizeSsmlToStreamAsync(ssml)).AsStream();
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
