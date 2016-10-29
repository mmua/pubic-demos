using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face.Contract;
using UWPKiosk.ViewModels;

namespace UWPKiosk.Services
{
    public class SimpleProductRecommender : IProductRecommender
    {
        public ProductRecommendation GetRecommendedUri(FaceViewModel face)
        {
            if (face != null && face.Age != null && face.Gender != null)
            {
                if (face.Age >= 0 && face.Age < 17)
                    return XboxOne;
                else if (face.Age < 23 && face.Gender == "female")
                    return Office365;
                else if (face.Age < 23 && face.Gender == "male")
                    return Lumia950;
                else if (face.Age < 30)
                    return SurfacePro;
                else if (face.Age < 40)
                    return SurfaceBook;
                else
                    return SurfaceStudio;
            }
            return AllTablets;
        }

        private static ProductRecommendation XboxOne => new ProductRecommendation
        {
            GreetingText = "Need more games? Find your game with XBox One!",
            GreetingLanguage = "en-US",
            ProductUrl = "https://www.microsoftstore.com/store/mseea/en_IE/pdp/Xbox-One-S-Minecraft-Favourites-Bundle-500GB/productID.5069326300"
        };

        private static ProductRecommendation Office365 => new ProductRecommendation
        {
            GreetingText = "Use the best tools while studing! Try new Office!",
            GreetingLanguage = "en-US",
            ProductUrl = "https://www.microsoftstore.com/store/mseea/en_IE/pdp/Office-365-University/productID.263156100?ICID=Office_365_ModF_365University"
        };

        private static ProductRecommendation Lumia950 => new ProductRecommendation
        {
            GreetingText = "Проявляйте гибкость! Пропробуйте Континуум вместе с Люмия 950!",
            GreetingLanguage = "ru-RU",
            ProductUrl = "https://www.microsoftstore.com/store/msusa/en_US/pdp/Microsoft-Lumia-950-XL---Unlocked/productID.326602300"
        };

        private static ProductRecommendation SurfacePro => new ProductRecommendation
        {
            GreetingText = "Universal devices for an incredible productivity! Try amazing Surface Pro 4!",
            GreetingLanguage = "en-US",
            ProductUrl = "https://www.microsoftstore.com/store/msusa/en_US/pdp/Microsoft-Surface-Pro-4/productID.5072641000"
        };

        private static ProductRecommendation SurfaceBook => new ProductRecommendation
        {
            GreetingText = "Laptop or tablet? No more need to choose! Try Surface Book!",
            GreetingLanguage = "en-US",
            ProductUrl = "https://www.microsoftstore.com/store/msusa/en_US/pdp/Surface-Book/productID.5072642200"
        };

        private static ProductRecommendation SurfaceStudio => new ProductRecommendation
        {
            GreetingText = "Make your home a studio! Try Surface Studio!",
            GreetingLanguage = "en-US",
            ProductUrl = "https://www.microsoftstore.com/store/msusa/en_US/pdp/productID.5074015900?icid=Cat-Surface-modD_40_3-Surface-102616-en_US"
        };

        private static ProductRecommendation AllTablets => new ProductRecommendation
        {
            GreetingText = "Check this amazing devices!",
            GreetingLanguage = "en-US",
            ProductUrl = "http://www.microsoftstore.com/store/msusa/en_US/cat/All-PCs-tablets/categoryID.69404700"
        };
    }
}
