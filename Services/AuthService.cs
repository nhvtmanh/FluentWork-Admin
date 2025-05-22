using FluentWork_Admin.Models;
using static System.Formats.Asn1.AsnWriter;

namespace FluentWork_Admin.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<M_Account_Login>> Login(M_Account_Login account);
        Task Logout();
        Task<ApiResponse<M_Account_Register>> Register(M_Account_Register account);
    }
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = factory.CreateClient("ApiClient");
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse<M_Account_Login>> Login(M_Account_Login account)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/login", account);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                var token = responseData!["access_token"];

                //Store token in session
                _httpContextAccessor.HttpContext?.Session.SetString("token", token);

                var successResponse = new ApiResponse<M_Account_Login>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = [ApiSuccessMessage.CREATE]
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Account_Login>>();
                return errorResponse!;
            }
        }
        public async Task Logout()
        {
            _httpContextAccessor.HttpContext?.Session.Remove("token");
        }
        public async Task<ApiResponse<M_Account_Register>> Register(M_Account_Register account)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/register", account);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_Account_Register>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = [ApiSuccessMessage.CREATE],
                    Data = await response.Content.ReadFromJsonAsync<M_Account_Register>(),
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Account_Register>>();
                return errorResponse!;
            }
        }
    }
}
