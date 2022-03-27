using System;
using Newtonsoft.Json;

namespace PlagueRecognition.Model
{
    public class Prediction
    {
        [JsonProperty("probability")]
        public double Probability { get; set; }

        [JsonProperty("tagId")]
        public Guid TagId { get; set; }

        [JsonProperty("tagName")]
        public string TagName { get; set; }
    }
}
