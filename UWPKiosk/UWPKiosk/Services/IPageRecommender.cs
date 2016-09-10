using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPKiosk.ViewModels;

namespace UWPKiosk.Services
{
    public interface IPageRecommender
    {
        string GetRecommendedUri(FaceViewModel faces);
    }
}
