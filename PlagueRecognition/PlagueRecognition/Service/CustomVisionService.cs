using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HttpExtension;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;
using PlagueRecognition.Constants;
using PlagueRecognition.Model;
using Plugin.Media.Abstractions;

namespace PlagueRecognition.Service
{
	public class CustomVisionService
	{
        public CustomVisionService()
        {

        }

        public async Task<string> GetPrediction(byte[] image)
        {
            try
            {
                var httpClient = new HttpClient();

                httpClient.DefaultRequestHeaders.Add("Prediction-Key", CustomVision.PredictionKey);

                var response = await httpClient.PostAsync<CustomVisionPrediction>($"{CustomVision.CustomVisionApiImage}", image, "application/octet-stream");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return response.Value.Predictions[0]?.TagName;
                }
                else
                {
                    Debug.WriteLine(response.Error.Message);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        public async Task<string> GetClassifyImage(MediaFile photo)
        {
            try
            {
                var endpoint = new CustomVisionPredictionClient
                {
                    ApiKey = CustomVision.PredictionKey,
                    Endpoint = CustomVision.CustomVisionEndPoint
                };

                var results = await endpoint.ClassifyImageAsync(Guid.Parse(CustomVision.ProjectId),
                                                              CustomVision.PublishName,
                                                              photo.GetStream());

                return results.Predictions?.OrderByDescending(x => x.Probability).FirstOrDefault().TagName;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }
    }
}