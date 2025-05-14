using FluentWork_Admin.Models;

namespace FluentWork_Admin.Services
{
    public interface ILessonService
    {
        public Task<ApiResponse<List<M_Lesson>>> GetList(string? topic, string? vocabularyTopic, string? grammarTopic, string? level);
        public Task<ApiResponse<M_Lesson>> GetById(int id);
        public Task<ApiResponse<M_Lesson>> Create(M_Lesson question);
        public Task<ApiResponse<M_Lesson>> Update(M_Lesson question);
        public Task<ApiResponse<M_Lesson>> Delete(int id);
    }
    public class LessonService : ILessonService
    {
        private readonly HttpClient _httpClient;

        public LessonService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<List<M_Lesson>>> GetList(string? type, string? vocabularyTopic, string? grammarTopic, string? level)
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

            var response = await _httpClient.GetAsync($"lessons?{string.Join("&", query.Select(kvp => $"{kvp.Key}={kvp.Value}"))}");

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<List<M_Lesson>>
                {
                    StatusCode = (int)response.StatusCode,
                    Data = await response.Content.ReadFromJsonAsync<List<M_Lesson>>(),
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<M_Lesson>>>();
                return errorResponse!;
            }
        }
        public async Task<ApiResponse<M_Lesson>> GetById(int id)
        {
            var response = await _httpClient.GetAsync($"lessons/{id}");

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_Lesson>
                {
                    StatusCode = (int)response.StatusCode,
                    Data = await response.Content.ReadFromJsonAsync<M_Lesson>(),
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Lesson>>();
                return errorResponse!;
            }
        }
        public async Task<ApiResponse<M_Lesson>> Create(M_Lesson lesson)
        {
            var data = new
            {
                title = lesson.Title,
                description = lesson.Description,
                type = lesson.Type,
                vocabulary_topic = lesson.VocabularyTopic,
                grammar_topic = lesson.GrammarTopic,
                level = lesson.Level,
                content = lesson.Content
            };

            var response = await _httpClient.PostAsJsonAsync("lessons", data);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_Lesson>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = [ApiSuccessMessage.CREATE],
                    Data = await response.Content.ReadFromJsonAsync<M_Lesson>(),
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Lesson>>();
                return errorResponse!;
            }
        }
        public async Task<ApiResponse<M_Lesson>> Update(M_Lesson lesson)
        {
            var data = new
            {
                title = lesson.Title,
                description = lesson.Description,
                type = lesson.Type,
                vocabulary_topic = lesson.VocabularyTopic,
                grammar_topic = lesson.GrammarTopic,
                level = lesson.Level,
                content = lesson.Content
            };

            var response = await _httpClient.PatchAsJsonAsync($"lessons/{lesson.Id}", data);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_Lesson>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = [ApiSuccessMessage.UPDATE],
                    Data = await response.Content.ReadFromJsonAsync<M_Lesson>(),
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Lesson>>();
                return errorResponse!;
            }
        }
        public async Task<ApiResponse<M_Lesson>> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"lessons/{id}");

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_Lesson>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = [ApiSuccessMessage.DELETE],
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Lesson>>();
                return errorResponse!;
            }
        }
    }
}
