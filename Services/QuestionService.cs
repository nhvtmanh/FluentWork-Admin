using FluentWork_Admin.Models;
using Microsoft.Extensions.Options;

namespace FluentWork_Admin.Services
{
    public interface IQuestionService
    {
        public Task<ApiResponse<List<M_Question>>> GetList(string? topic, string? vocabularyTopic, string? grammarTopic, string? level);
        public Task<ApiResponse<M_Question>> GetById(int id);
        public Task<ApiResponse<M_Question>> Create(M_Question question);
        public Task<ApiResponse<M_Question>> Update(M_Question question);
    }
    public class QuestionService : IQuestionService
    {
        private readonly HttpClient _httpClient;

        public QuestionService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<List<M_Question>>> GetList(string? type, string? vocabularyTopic, string? grammarTopic, string? level)
        {
            var query = new Dictionary<string, string>
            {
                { "type", type! },
                { "vocabulary_topic", vocabularyTopic! },
                { "grammar_topic", grammarTopic! },
                { "level", level! }
            };

            // Remove null or empty values from the query
            query = query.Where(kvp => !string.IsNullOrEmpty(kvp.Value))
                         .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var response = await _httpClient.GetAsync($"questions?{string.Join("&", query.Select(kvp => $"{kvp.Key}={kvp.Value}"))}");

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<List<M_Question>>
                {
                    StatusCode = (int)response.StatusCode,
                    Data = await response.Content.ReadFromJsonAsync<List<M_Question>>(),
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<M_Question>>>();
                return errorResponse!;
            }
        }
        public async Task<ApiResponse<M_Question>> GetById(int id)
        {
            var response = await _httpClient.GetAsync($"questions/{id}/options");

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_Question>
                {
                    StatusCode = (int)response.StatusCode,
                    Data = await response.Content.ReadFromJsonAsync<M_Question>(),
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Question>>();
                return errorResponse!;
            }
        }
        public async Task<ApiResponse<M_Question>> Create(M_Question question)
        {
            var data = new
            {
                type = question.Type,
                vocabulary_topic = question.VocabularyTopic,
                grammar_topic = question.GrammarTopic,
                level = question.Level,
                question_text = question.QuestionText,
                explanation = question.Explanation,
                options = question.Options.Select(o => new
                {
                    option_text = o.OptionText,
                    is_correct = o.IsCorrect
                }).ToList()
            };

            var response = await _httpClient.PostAsJsonAsync("questions", data);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_Question>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = [ApiSuccessMessage.CREATE],
                    Data = await response.Content.ReadFromJsonAsync<M_Question>(),
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Question>>();
                return errorResponse!;
            }
        }
        public async Task<ApiResponse<M_Question>> Update(M_Question question)
        {
            var data = new
            {
                type = question.Type,
                vocabulary_topic = question.VocabularyTopic,
                grammar_topic = question.GrammarTopic,
                level = question.Level,
                question_text = question.QuestionText,
                explanation = question.Explanation,
                options = question.Options.Select(o => new
                {
                    option_text = o.OptionText,
                    is_correct = o.IsCorrect
                }).ToList()
            };

            var response = await _httpClient.PatchAsJsonAsync($"questions/{question.Id}", data);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_Question>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = [ApiSuccessMessage.UPDATE],
                    Data = await response.Content.ReadFromJsonAsync<M_Question>(),
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Question>>();
                return errorResponse!;
            }
        }
    }
}
