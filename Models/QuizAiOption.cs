using Newtonsoft.Json;

namespace MandarinQuest.Models
{
    public class QuizAiOption
    {
        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}