﻿using FluentWork_Admin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FluentWork_Admin.Services
{
    public interface IUserService
    {
        public Task<ApiResponse<List<M_User>>> GetList(string? role);
        public Task<ApiResponse<M_User>> GetById(int id);
        public Task<ApiResponse<M_User>> Create(M_User user);
        public Task<ApiResponse<M_User>> Update(M_User user);
        public Task<ApiResponse<M_User>> Delete(int id);
    }
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ApiClient");
        }

        public async Task<ApiResponse<List<M_User>>> GetList(string? role)
        {
            var query = new Dictionary<string, string>
            {
                { "role", role! }
            };

            // Remove null or empty values from the query
            query = query.Where(kvp => !string.IsNullOrEmpty(kvp.Value))
                         .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var response = await _httpClient.GetAsync($"users?{string.Join("&", query.Select(kvp => $"{kvp.Key}={kvp.Value}"))}");

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<dynamic>(responseData);

                var successResponse = new ApiResponse<List<M_User>>
                {
                    StatusCode = (int)response.StatusCode,
                    Data = ((JArray)data!.users).ToObject<List<M_User>>()
                };
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<M_User>>>();
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
        public async Task<ApiResponse<M_User>> Create(M_User user)
        {
            var data = new
            {
                username = user.Username,
                email = user.Email,
                fullname = user.Fullname,
                password = user.Password,
                role = user.Role
            };

            var response = await _httpClient.PostAsJsonAsync("users", data);

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_User>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = [ApiSuccessMessage.CREATE],
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
                return successResponse;
            }
            else
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<ApiResponse<M_User>>();
                return errorResponse!;
            }
        }
        public async Task<ApiResponse<M_User>> Delete(int id)
        {
            var response = await _httpClient.DeleteAsync($"users/{id}");

            if (response.IsSuccessStatusCode)
            {
                var successResponse = new ApiResponse<M_User>
                {
                    StatusCode = (int)response.StatusCode,
                    Message = [ApiSuccessMessage.DELETE],
                };
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
