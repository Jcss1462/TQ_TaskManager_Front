using System.Text.Json;
using System.Text;
using TQ_TaskManager_Back.Dtos;

namespace TQ_TaskManager_Front.Services
{
    public class AuthService: IAuthService
    {

        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LoginAsync(LoginRequestModel input)
        {
            var content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonSerializer.Deserialize<LoginResponseModel>(responseContent);

                if (responseObject?.token != null)
                {
                    StoreToken(responseObject.token);
                    return;
                }

                throw new Exception("No token received from the server.");
            }

            throw new Exception($"Login failed with status code: {response.StatusCode}");
        }

        public string? GetToken()
        {
            return _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
        }

        private void StoreToken(string token)
        {
            _httpContextAccessor.HttpContext?.Session.SetString("JwtToken", token);
        }

    }

    public interface IAuthService
    {
        Task LoginAsync(LoginRequestModel input);
        string? GetToken();
    }
}
