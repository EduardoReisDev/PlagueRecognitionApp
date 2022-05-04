using System;
namespace PlagueRecognition.Constants
{
    public class CustomVision
    {
        public static string CustomVisionApiUrl => "https://southcentralus.api.cognitive.microsoft.com/customvision/v3.0/Prediction/37d503a6-0baa-4f40-839f-b5523e8715e6/classify/iterations/Principal/image";
        public static string PredictionKey => "6c049996cdc14b37a285b07ff66adc87";
        public static Guid ProjectId = new Guid("37d503a6-0baa-4f40-839f-b5523e8715e6");
        public static string PublishName => "PlagueRecognition";
    }
}
