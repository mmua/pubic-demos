using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherBotDemo.Services.OpenWeatherMap
{
    public class WeatherRecord
    {
        public string City { get; set; }
        public double Temp { get; set; }
        public double Pressure { get; set; }
        public int Humidity { get; set; }
        public DateTime When { get; set; }
        public string ShortDate
        {
            get { return $"{When.Day:D2}.{When.Month:D2}"; }
        }

        public string FullDate
        {
            get { return When.ToString("D"); }
        }

        public override string ToString()
        {
            return $"{ShortDate}: {Temp}°C, {Pressure} Pa, {Humidity}%";
        }
    }
}