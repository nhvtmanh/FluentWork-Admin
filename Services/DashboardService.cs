using FluentWork_Admin.Models;

namespace FluentWork_Admin.Services
{
    public interface IDashboardService
    {
        Task<ApiResponse<object>> GetSummary();
        Task<ApiResponse<object>> GetLearnersDaily();
        Task<ApiResponse<object>> GetRoles();
    }
    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _httpClient;

        public DashboardService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<object>> GetSummary()
        {
            var response = await _httpClient.GetAsync("dashboard/summary");

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<object>
                {
                    StatusCode = (int)response.StatusCode,
                    Data = await response.Content.ReadFromJsonAsync<object>()
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                return errorResponse!;
            }
        }
        public async Task<ApiResponse<object>> GetLearnersDaily()
        {
            var response = await _httpClient.GetAsync("dashboard/learners-daily");

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<object>
                {
                    StatusCode = (int)response.StatusCode,
                    Data = await response.Content.ReadFromJsonAsync<object>()
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                return errorResponse!;
            }
        }
        public async Task<ApiResponse<object>> GetRoles()
        {
            var response = await _httpClient.GetAsync("dashboard/user-role-distribution");

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<object>
                {
                    StatusCode = (int)response.StatusCode,
                    Data = await response.Content.ReadFromJsonAsync<object>()
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
                return errorResponse!;
            }
        }

    }
}
