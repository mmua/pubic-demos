using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPKiosk.ViewModels;

namespace UWPKiosk.Services
{
    public interface IProductRecommender
    {
        ProductRecommendation GetRecommendedUri(FaceViewModel faces);
    }

    public class ProductRecommendation
    {
        public string GreetingText { get; set; }
        public string GreetingLanguage { get; set; }
        public string ProductUrl { get; set; }
    }
}
