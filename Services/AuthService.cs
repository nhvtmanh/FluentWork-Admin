﻿using FluentWork_Admin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace FluentWork_Admin.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<M_Account_Login>> Login(M_Account_Login account);
        Task Logout();
        Task<ApiResponse<M_Account_ForgotPassword>> ForgotPassword(M_Account_ForgotPassword account);
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

                //Store token in session
                _httpContextAccessor.HttpContext?.Session.SetString("token", token);

                var successResponse = new ApiResponse<M_Account_Login>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = ((JArray)data.message).ToObject<List<string>>()!
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
        public async Task<ApiResponse<M_Account_ForgotPassword>> ForgotPassword(M_Account_ForgotPassword account)
        {
            var response = await _httpClient.PostAsJsonAsync("auth/forgot-password", account);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_Account_ForgotPassword>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = ["Password changed successfully"]
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_Account_ForgotPassword>>();
                return errorResponse!;
            }
        }
    }
}
