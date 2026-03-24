using MandarinQuest.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MandarinQuest.Services
{
    public class GeminiService
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<string> AskGeminiAsync(string userQuestion)
        {
            string apiKey = ConfigurationManager.AppSettings["Gemini:ApiKey"];
            string model = ConfigurationManager.AppSettings["Gemini:Model"];
            string baseUrl = ConfigurationManager.AppSettings["Gemini:BaseUrl"];

            if (string.IsNullOrWhiteSpace(apiKey) ||
                string.IsNullOrWhiteSpace(model) ||
                string.IsNullOrWhiteSpace(baseUrl))
            {
                return "Gemini configuration is missing.";
            }

            string url = baseUrl + model + ":generateContent?key=" + apiKey;

            var requestBody = new GeminiRequest
            {
                contents = new System.Collections.Generic.List<Content>
                {
                    new Content
                    {
                        parts = new System.Collections.Generic.List<Part>
                        {
                            new Part
                            {
                                text = "You are a helpful Mandarin tutor for beginners. " +
                                   "Give short, clear answers. Include pinyin when useful. " +
                                   "Do not use markdown, asterisks, bold formatting, or bullet symbols. " +
                                   "Plain text only. " +
                                   "User question: " + userQuestion
                            }
                        }
                    }
                }
            };

            string json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);
                string responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return "Gemini API error: " + response.StatusCode + "<br/>" + responseJson;
                }

                var result = JsonConvert.DeserializeObject<GeminiResponse>(responseJson);

                if (result != null &&
                    result.candidates != null &&
                    result.candidates.Count > 0 &&
                    result.candidates[0].content != null &&
                    result.candidates[0].content.parts != null &&
                    result.candidates[0].content.parts.Count > 0)
                {
                    return result.candidates[0].content.parts[0].text;
                }

                return "No response received from Gemini.";
            }
            catch (Exception ex)
            {
                return "Error calling Gemini: " + ex.Message;
            }
        }
    }
}