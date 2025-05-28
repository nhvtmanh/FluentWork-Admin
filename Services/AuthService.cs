using FluentWork_Admin.Common;
using FluentWork_Admin.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;

namespace FluentWork_Admin.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<M_Account_Login>> Login(M_Account_Login account);
        ApiResponse<object> Logout();
        Task<ApiResponse<M_Account_ForgotPassword>> ForgotPassword(M_Account_ForgotPassword account);
        Task<ApiResponse<M_User>> GetById(int id);
        Task<ApiResponse<M_User>> Update(M_User user);
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
                string responseData = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(responseData);
                string token = data!.access_token;

                //Decode token using JsonWebTokenHandler
                var handler = new JsonWebTokenHandler();
                var jwt = handler.ReadJsonWebToken(token);
                var role = jwt.Claims.FirstOrDefault(c => c.Type == "role")?.Value;

                if (role != "Admin")
                {
                    return new ApiResponse<M_Account_Login>
                    {
                        StatusCode = StatusCodes.Status401Unauthorized,
                        Message = ["You do not have permission to access this resource!"]
                    };
                }

                //Store token in session
                _httpContextAccessor.HttpContext?.Session.SetString("token", token);
                AccountInfo.AccountId = int.Parse(jwt.Claims.FirstOrDefault(c => c.Type == "sub")?.Value!);
                AccountInfo.Username = jwt.Claims.FirstOrDefault(c => c.Type == "username")?.Value!;

                var successResponse = new ApiResponse<M_Account_Login>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = ["Login successfully!"]
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Account_Login>>();
                return errorResponse!;
            }
        }
        public ApiResponse<object> Logout()
        {
            _httpContextAccessor.HttpContext?.Session.Remove("token");

            return new ApiResponse<object>
            {
                StatusCode = StatusCodes.Status200OK,
                Message = ["Logout successfully!"]
            };
        }
        public async Task<ApiResponse<M_Account_ForgotPassword>> ForgotPassword(M_Account_ForgotPassword account)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/forgot-password", account);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_Account_ForgotPassword>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = ["Password changed successfully!"]
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Account_ForgotPassword>>();
                return errorResponse!;
            }
        }
        public async Task<ApiResponse<M_User>> GetById(int id)
        {
            var response = await _httpClient.GetAsync($"users/{id}");

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_User>
                {
                    StatusCode = (int)response.StatusCode,
                    Data = await response.Content.ReadFromJsonAsync<M_User>(),
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_User>>();
                return errorResponse!;
            }
        }
        public async Task<ApiResponse<M_User>> Update(M_User user)
        {
            var data = new
            {
                username = user.Username,
                email = user.Email,
                fullname = user.Fullname,
                role = user.Role
            };

            var response = await _httpClient.PatchAsJsonAsync($"users/{user.Id}", data);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_User>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = [ApiSuccessMessage.UPDATE],
                    Data = await response.Content.ReadFromJsonAsync<M_User>(),
                };

                AccountInfo.Username = data.username;

                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_User>>();
                return errorResponse!;
            }
        }
    }
}
