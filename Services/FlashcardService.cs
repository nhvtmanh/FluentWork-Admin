using FluentWork_Admin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FluentWork_Admin.Services
{
    public interface IFlashcardService
    {
        public Task<ApiResponse<List<M_Flashcard>>> GetList(string? topic);
        public Task<ApiResponse<M_Flashcard>> GetById(int id);
        public Task<ApiResponse<M_Flashcard>> Create(M_Flashcard flashcard);
        public Task<ApiResponse<M_Flashcard>> Update(M_Flashcard flashcard);
        public Task<ApiResponse<M_Flashcard>> Delete(int id);
    }
    public class FlashcardService : IFlashcardService
    {
        private readonly HttpClient _httpClient;

        public FlashcardService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<List<M_Flashcard>>> GetList(string? topic)
        {
            var query = new Dictionary<string, string>
            {
                { "topic", topic! }
            };

            // Remove null or empty values from the query
            query = query.Where(kvp => !string.IsNullOrEmpty(kvp.Value))
                         .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var response = await _httpClient.GetAsync($"flashcards?{string.Join("&", query.Select(kvp => $"{kvp.Key}={kvp.Value}"))}");

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(responseData);
                
                var successResponse = new ApiResponse<List<M_Flashcard>>
                {
                    StatusCode = (int)response.StatusCode,
                    Data = ((JArray)data!.flashcards).ToObject<List<M_Flashcard>>()
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<M_Flashcard>>>();
                return errorResponse!;
            }
        }
        public async Task<ApiResponse<M_Flashcard>> GetById(int id)
        {
            var response = await _httpClient.GetAsync($"flashcards/{id}");

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_Flashcard>
                {
                    StatusCode = (int)response.StatusCode,
                    Data = await response.Content.ReadFromJsonAsync<M_Flashcard>(),
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Flashcard>>();
                return errorResponse!;
            }
        }
        public async Task<ApiResponse<M_Flashcard>> Create(M_Flashcard flashcard)
        {
            var data = new
            {
                topic = flashcard.Topic,
                word = flashcard.Word,
                definition = flashcard.Definition
            };

            var response = await _httpClient.PostAsJsonAsync("flashcards", data);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_Flashcard>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = [ApiSuccessMessage.CREATE],
                    Data = await response.Content.ReadFromJsonAsync<M_Flashcard>(),
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Flashcard>>();
                return errorResponse!;
            }
        }
        public async Task<ApiResponse<M_Flashcard>> Update(M_Flashcard flashcard)
        {
            var data = new
            {
                topic = flashcard.Topic,
                word = flashcard.Word,
                definition = flashcard.Definition
            };

            var response = await _httpClient.PatchAsJsonAsync($"flashcards/{flashcard.Id}", data);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_Flashcard>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = [ApiSuccessMessage.UPDATE],
                    Data = await response.Content.ReadFromJsonAsync<M_Flashcard>(),
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Flashcard>>();
                return errorResponse!;
            }
        }
        public async Task<ApiResponse<M_Flashcard>> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"flashcards/{id}");

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_Flashcard>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = [ApiSuccessMessage.DELETE],
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Flashcard>>();
                return errorResponse!;
            }
        }
    }
}
