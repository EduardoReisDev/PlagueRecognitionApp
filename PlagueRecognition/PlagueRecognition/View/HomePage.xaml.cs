using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace PlagueRecognition.View
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private async void OpenCamera(object sender, EventArgs e)
        {
            var imageStream = await PickImage();

            if (imageStream == null)
                return;

            var client = new CustomVisionPredictionClient
            {
                ApiKey = Constants.CustomVision.PredictionKey,
                Endpoint = Constants.CustomVision.CustomVisionApiUrl
            };

            var result = await client.ClassifyImageAsync(Constants.CustomVision.ProjectId, Constants.CustomVision.PublishName, imageStream);
            var bestResult = result.Predictions.OrderByDescending(p => p.Probability).FirstOrDefault();

            if (bestResult == null)
                return;

            //SetLabel($"{bestResult.TagName} ({Math.Round(bestResult.Probability * 100, 2)}%)");
        }

        private async Task<Stream> PickImage()
        {
            await CrossMedia.Current.Initialize();

            var pickOrCamera = await DisplayActionSheet("Do you want to pick a photo from the gallery or take a picture with the camera?", "Cancel", null, "Pick from gallery", "Take with camera");

            MediaFile pickedImage = null;

            switch (pickOrCamera)
            {
                case "Cancel":
                    return null;
                case "Pick from gallery":
                    pickedImage = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                    {
                        PhotoSize = PhotoSize.Small
                    });
                    break;
                case "Take with camera":
                    if (!CrossMedia.Current.IsCameraAvailable)
                    {
                        await DisplayAlert("Camera not available", "Sorry, the camera does not seem available. Maybe you are running this app on an emulator or Simulator where the camera view is not supported?", "OK");

                        break;
                    }

                    pickedImage = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        PhotoSize = PhotoSize.Small,
                        SaveToAlbum = false
                    });
                    break;
            }

            if (pickedImage == null)
                return null;

            return pickedImage.GetStreamWithImageRotatedForExternalStorage();
        }
    }
}
