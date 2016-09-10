using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Face.Contract;
using UWPKiosk.ViewModels;

namespace UWPKiosk.Services
{
    public class SimplePageRecommender : IPageRecommender
    {
        public string GetRecommendedUri(FaceViewModel face)
        {
            if (face != null && face.Age != null && face.Gender != null)
            {
                if (face.Age >= 0 && face.Age < 17)
                    return "http://www.microsoftstore.com/store/msusa/en_US/pdp/Xbox-One-+-Kinect-Bundle/productID.330348300";
                else if (face.Age >= 15 && face.Age < 23 && face.Gender == "female")
                    return "http://www.microsoftstore.com/store/msusa/en_US/pdp/Office-365-University/productID.275549300";
                else if (face.Age >= 15 && face.Age < 23 && face.Gender == "male")
                    return "http://www.microsoftstore.com/store/msusa/en_US/pdp/productID.324438600";
                else if (face.Age >= 23 && face.Age < 30)
                    return "http://www.microsoftstore.com/store/msusa/en_US/pdp/Microsoft-Lumia-950--Unlocked/productID.326602600";
                else if (face.Age >= 30 && face.Age < 40)
                    return "http://www.microsoftstore.com/store/msusa/en_US/pdp/productID.325711500";
                else if (face.Age >= 40)
                    return "http://www.microsoftstore.com/store/msusa/en_US/pdp/productID.325716000";
            }
            return "http://www.microsoftstore.com/store/msusa/en_US/cat/All-PCs-tablets/categoryID.69404700";
        }
    }
}
