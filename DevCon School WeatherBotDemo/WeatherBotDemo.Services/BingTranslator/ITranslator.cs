using System.Threading.Tasks;

namespace WeatherBotDemo.Services.BingTranslator
{
    public interface ITranslator
    {
        Task<string> Translate(string text, string fromLocale, string toLocale);
    }
}