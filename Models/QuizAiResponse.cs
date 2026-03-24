using System.Collections.Generic;
using Newtonsoft.Json;

namespace MandarinQuest.Models
{
    public class QuizAiResponse
    {
        [JsonProperty("quizTitle")]
        public string QuizTitle { get; set; }

        [JsonProperty("questions")]
        public List<QuizAiQuestion> Questions { get; set; } = new List<QuizAiQuestion>();
    }
}