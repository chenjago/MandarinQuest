using System.Collections.Generic;
using Newtonsoft.Json;

namespace MandarinQuest.Models
{
    public class QuizAiQuestion
    {
        [JsonProperty("questionText")]
        public string QuestionText { get; set; }

        [JsonProperty("correctOption")]
        public string CorrectOption { get; set; }

        [JsonProperty("explanation")]
        public string Explanation { get; set; }

        [JsonProperty("options")]
        public List<QuizAiOption> Options { get; set; } = new List<QuizAiOption>();
    }
}