using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.ProjectOxford.Face;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using UWPKiosk.Services;
using System.Collections.ObjectModel;
using Windows.UI.Core;

namespace UWPKiosk.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private FaceServiceClient _faceClient = new FaceServiceClient("82d432b90f684eebbd03f2015265d286");
        private IPageRecommender _recommender = new SimplePageRecommender();

        private SoftwareBitmapSource _photo = null;
        public SoftwareBitmapSource Photo
        {
            get { return _photo; }
            set { Set(() => Photo, ref _photo, value); }
        }

        private string _jsonResponse = "Choose a photo using buttons below";
        public string JsonResponse
        {
            get { return _jsonResponse; }
            set { Set(() => JsonResponse, ref _jsonResponse, value); }
        }

        private string _uri = "http://www.microsoftstore.com/store/msusa/en_US/home";
        public string Uri
        {
            get { return _uri; }
            set { Set(() => Uri, ref _uri, value); }
        }

        private bool _isPhotoUnderProcessing = false;
        public bool IsPhotoUnderProcessing
        {
            get { return _isPhotoUnderProcessing; }
            set { Set(() => IsPhotoUnderProcessing, ref _isPhotoUnderProcessing, value); }
        }

        private ObservableCollection<FaceViewModel> _detectedFaces = null;
        public ObservableCollection<FaceViewModel> DetectedFaces
        {
            get { return _detectedFaces; }
            set { Set(() => DetectedFaces, ref _detectedFaces, value); }
        }

        private object _selectedFace = null;
        public object SelectedFace
        {
            get { return _selectedFace; }
            set
            {
                Set(() => SelectedFace, ref _selectedFace, value);
                Uri = _recommender.GetRecommendedUri((FaceViewModel)value);
            }
        }

        public RelayCommand<string> TakePhotoCommand => new RelayCommand<string>(TakePhoto);
        private async void TakePhoto(string operationType)
        {
            try
            {
                if (operationType == null) throw new ArgumentNullException(nameof(operationType));

                StorageFile photo = null;
                switch (operationType)
                {
                    case "capture":
                        photo = await CapturePhoto();
                        break;
                    case "library":
                        photo = await PickPhoto();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(operationType), "Unknown value: " + (operationType ?? "NULL"));
                }

                if (photo == null)
                    return;

                Photo = await PreparePhoto(photo);

                IsPhotoUnderProcessing = true;
                var faces = await _faceClient.DetectAsync((await photo.OpenAsync(FileAccessMode.Read)).AsStream(), returnFaceAttributes: Enum.GetValues(typeof(FaceAttributeType)).Cast<FaceAttributeType>());
                DetectedFaces = new ObservableCollection<FaceViewModel>(await Task.WhenAll(faces.Select(f => FaceViewModel.FromFace(f, photo))));
                SelectedFace = DetectedFaces.FirstOrDefault();
                IsPhotoUnderProcessing = false;
            }
            catch (Exception e)
            {
                var dialog = new ContentDialog
                {
                    Title = "Unexpected error",
                    PrimaryButtonText = "Ok",
                    Content = new TextBlock
                    {
                        Text = e.Message,
                        TextWrapping = Windows.UI.Xaml.TextWrapping.WrapWholeWords
                    }
                };
                await dialog.ShowAsync();
            }
        }

        private async Task<SoftwareBitmapSource> PreparePhoto(StorageFile photo)
        {
            IRandomAccessStream stream = await photo.OpenAsync(FileAccessMode.Read);
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
            SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();

            SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
            SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
            await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

            return bitmapSource;
        }

        private static async Task<StorageFile> PickPhoto()
        {
            var filePicker = new FileOpenPicker();
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.CommitButtonText = "Choose";

            return await filePicker.PickSingleFileAsync();
        }

        private static async Task<StorageFile> CapturePhoto()
        {
            var captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(500, 500);

            return await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);
        }
    }
}
