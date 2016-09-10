using GalaSoft.MvvmLight;
using Microsoft.ProjectOxford.Face.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPKiosk.ViewModels
{
    public class FaceViewModel : ViewModelBase
    {
        private SoftwareBitmapSource _photo;
        public SoftwareBitmapSource Photo
        {
            get { return _photo; }
            set { Set(() => Photo, ref _photo, value); }
        }

        private double? _age;
        public double? Age
        {
            get { return _age; }
            set { Set(() => Age, ref _age, value); }
        }

        private string _gender;
        public string Gender
        {
            get { return _gender; }
            set { Set(() => Gender, ref _gender, value); }
        }

        private double? _smile;
        public double? Smile
        {
            get { return _smile; }
            set { Set(() => Smile, ref _smile, value); }
        }

        private double? _beard;
        public double? Beard
        {
            get { return _beard; }
            set { Set(() => Beard, ref _beard, value); }
        }

        private double? _moustache;
        public double? Moustache
        {
            get { return Moustache; }
            set { Set(() => Moustache, ref _moustache, value); }
        }

        private double? _sideburns;
        public double? Sideburns
        {
            get { return _sideburns; }
            set { Set(() => Sideburns, ref _sideburns, value); }
        }

        public static async Task<FaceViewModel> FromFace(Face face, StorageFile photo)
        {
            if (face == null) throw new ArgumentNullException(nameof(face));
            return new FaceViewModel
            {
                Photo = await CropPhoto(photo, face.FaceRectangle),
                Age = face.FaceAttributes?.Age,
                Gender = face.FaceAttributes?.Gender,
                Smile = face.FaceAttributes?.Smile,
                Beard = face.FaceAttributes?.FacialHair?.Beard,
                Moustache = face.FaceAttributes?.FacialHair?.Moustache,
                Sideburns = face.FaceAttributes?.FacialHair?.Sideburns
            };
        }

        private static async Task<SoftwareBitmapSource> CropPhoto(StorageFile photo, FaceRectangle rectangle)
        {
            using (var imageStream = await photo.OpenReadAsync())
            {
                var decoder = await BitmapDecoder.CreateAsync(imageStream);
                if (decoder.PixelWidth >= rectangle.Left + rectangle.Width || decoder.PixelHeight >= rectangle.Top + rectangle.Height)
                {
                    var transform = new BitmapTransform
                    {
                        Bounds = new BitmapBounds
                        {
                            X = (uint)rectangle.Left,
                            Y = (uint)rectangle.Top,
                            Height = (uint)rectangle.Height,
                            Width = (uint)rectangle.Width
                        }
                    };

                    var softwareBitmapBGR8 = await decoder.GetSoftwareBitmapAsync(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, transform, ExifOrientationMode.IgnoreExifOrientation, ColorManagementMode.DoNotColorManage);
                    SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
                    await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

                    return bitmapSource;
                }
                return null;
            }
        }
    }
}
