using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MandarinQuest.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MandarinQuest.Services
{
    public class OpenAIQuizService
    {
        private readonly string _apiKey;
        private readonly string _model;
        private readonly string _baseUrl;

        public OpenAIQuizService()
        {
            _apiKey = ConfigurationManager.AppSettings["Gemini:ApiKey"];
            _model = ConfigurationManager.AppSettings["Gemini:Model"] ?? "gemini-2.5-flash";
            _baseUrl = ConfigurationManager.AppSettings["Gemini:BaseUrl"] ?? "https://generativelanguage.googleapis.com/v1beta/models/";

            if (string.IsNullOrWhiteSpace(_apiKey) ||
                _apiKey.Contains("PASTE_YOUR_NEW_GEMINI_API_KEY_HERE"))
            {
                throw new InvalidOperationException("Missing Gemini API key in Web.config appSettings.");
            }
        }

        public async Task<QuizAiResponse> GenerateQuizAsync(string lessonTitle, string extractedMaterialText, int questionCount = 5)
        {
            if (string.IsNullOrWhiteSpace(lessonTitle))
                throw new ArgumentException("Lesson title is required.", nameof(lessonTitle));

            if (string.IsNullOrWhiteSpace(extractedMaterialText))
                throw new ArgumentException("Extracted material text is empty.", nameof(extractedMaterialText));

            string limitedMaterialText = TrimToSafeLength(extractedMaterialText, 15000);

            var requestBody = BuildRequestBody(lessonTitle, limitedMaterialText, questionCount);
            string rawResponse = await SendRequestAsync(requestBody);
            QuizAiResponse quiz = ParseQuizResponse(rawResponse);

            ValidateQuizResponse(quiz, questionCount);
            return quiz;
        }

        private object BuildRequestBody(string lessonTitle, string materialText, int questionCount)
        {
            string prompt =
                "Create a multiple-choice quiz for this Mandarin lesson.\n\n" +
                "Lesson title: " + lessonTitle + "\n" +
                "Required question count: " + questionCount + "\n\n" +
                "Rules:\n" +
                "- Return exactly " + questionCount + " questions.\n" +
                "- Each question must have exactly 4 options labeled A, B, C, D.\n" +
                "- Each question must have exactly one correct option.\n" +
                "- Keep every question based only on the lesson material.\n" +
                "- Use simple English.\n" +
                "- Add a short explanation for the correct answer.\n\n" +
                "Use the lesson materials below:\n" +
                materialText;

            return new
            {
                systemInstruction = new
                {
                    parts = new object[]
                    {
                        new
                        {
                            text =
                                "You generate lesson quizzes for a Mandarin learning system. " +
                                "Return valid JSON only. " +
                                "Do not include markdown, code fences, or extra commentary."
                        }
                    }
                },
                contents = new object[]
                {
                    new
                    {
                        role = "user",
                        parts = new object[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.3,
                    maxOutputTokens = 4096,
                    responseMimeType = "application/json",
                    responseSchema = new
                    {
                        type = "OBJECT",
                        properties = new
                        {
                            quizTitle = new
                            {
                                type = "STRING"
                            },
                            questions = new
                            {
                                type = "ARRAY",
                                minItems = questionCount,
                                maxItems = questionCount,
                                items = new
                                {
                                    type = "OBJECT",
                                    properties = new
                                    {
                                        questionText = new
                                        {
                                            type = "STRING"
                                        },
                                        correctOption = new
                                        {
                                            type = "STRING",
                                            @enum = new[] { "A", "B", "C", "D" }
                                        },
                                        explanation = new
                                        {
                                            type = "STRING"
                                        },
                                        options = new
                                        {
                                            type = "ARRAY",
                                            minItems = 4,
                                            maxItems = 4,
                                            items = new
                                            {
                                                type = "OBJECT",
                                                properties = new
                                                {
                                                    label = new
                                                    {
                                                        type = "STRING",
                                                        @enum = new[] { "A", "B", "C", "D" }
                                                    },
                                                    text = new
                                                    {
                                                        type = "STRING"
                                                    }
                                                },
                                                required = new[] { "label", "text" }
                                            }
                                        }
                                    },
                                    required = new[] { "questionText", "correctOption", "explanation", "options" }
                                }
                            }
                        },
                        required = new[] { "quizTitle", "questions" }
                    }
                }
            };
        }

        private async Task<string> SendRequestAsync(object requestBody)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(90);
                client.DefaultRequestHeaders.Add("x-goog-api-key", _apiKey);

                string endpoint = _baseUrl.TrimEnd('/') + "/" + _model + ":generateContent";
                string json = JsonConvert.SerializeObject(requestBody);

                using (StringContent content = new StringContent(json, Encoding.UTF8, "application/json"))
                {
                    HttpResponseMessage response = await client.PostAsync(endpoint, content);
                    string responseText = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                        throw new Exception("Gemini API error: " + (int)response.StatusCode + " - " + responseText);

                    return responseText;
                }
            }
        }

        private QuizAiResponse ParseQuizResponse(string rawResponse)
        {
            JObject root = JObject.Parse(rawResponse);

            string content =
                root["candidates"]?
                    .FirstOrDefault()?["content"]?["parts"]?
                    .FirstOrDefault()?["text"]?
                    .ToString();

            if (string.IsNullOrWhiteSpace(content))
                throw new Exception("Gemini response did not contain message content.");

            QuizAiResponse quiz = JsonConvert.DeserializeObject<QuizAiResponse>(content);

            if (quiz == null)
                throw new Exception("Failed to deserialize quiz JSON.");

            return quiz;
        }

        private void ValidateQuizResponse(QuizAiResponse quiz, int expectedQuestionCount)
        {
            if (quiz == null)
                throw new Exception("Quiz response is null.");

            if (string.IsNullOrWhiteSpace(quiz.QuizTitle))
                throw new Exception("Quiz title is missing.");

            if (quiz.Questions == null || quiz.Questions.Count != expectedQuestionCount)
                throw new Exception("Quiz question count is invalid.");

            foreach (QuizAiQuestion question in quiz.Questions)
            {
                if (string.IsNullOrWhiteSpace(question.QuestionText))
                    throw new Exception("A question is missing question text.");

                question.CorrectOption = (question.CorrectOption ?? string.Empty).Trim().ToUpperInvariant();

                if (!new[] { "A", "B", "C", "D" }.Contains(question.CorrectOption))
                    throw new Exception("A question has an invalid correct option.");

                if (question.Options == null || question.Options.Count != 4)
                    throw new Exception("A question does not have exactly 4 options.");

                string[] labels = question.Options
                    .Select(o => (o.Label ?? string.Empty).Trim().ToUpperInvariant())
                    .OrderBy(x => x)
                    .ToArray();

                string actual = string.Join(",", labels);
                string expected = "A,B,C,D";

                if (actual != expected)
                    throw new Exception("Question options must be exactly A, B, C, D.");

                foreach (QuizAiOption option in question.Options)
                {
                    option.Label = (option.Label ?? string.Empty).Trim().ToUpperInvariant();
                    option.Text = (option.Text ?? string.Empty).Trim();

                    if (string.IsNullOrWhiteSpace(option.Text))
                        throw new Exception("An option is missing text.");
                }

                if (string.IsNullOrWhiteSpace(question.Explanation))
                    question.Explanation = "No explanation provided.";
            }
        }

        private string TrimToSafeLength(string text, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            text = text.Trim();

            if (text.Length <= maxLength)
                return text;

            return text.Substring(0, maxLength);
        }
    }
}