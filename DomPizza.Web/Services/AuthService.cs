using Blazored.LocalStorage;
using DomPizza.Web.Models;
using System.Net.Http.Json;
using System.Net.Http.Headers;

namespace DomPizza.Web.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly ILocalStorageService _localStorage;
        private readonly CustomAuthStateProvider _authProvider;

        public AuthService(HttpClient http, ILocalStorageService localStorage, CustomAuthStateProvider authProvider)
        {
            _http = http;
            _localStorage = localStorage;
            _authProvider = authProvider;
        }

        public async Task<bool> LoginAsync(LoginDTO dto)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("login", dto);

                if (!response.IsSuccessStatusCode)
                {
                    var msg = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Login falhou: {msg}");
                    return false;
                }

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result == null || string.IsNullOrEmpty(result.Token))
                    return false;

                await _localStorage.SetItemAsync("token", result.Token);

                _http.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", result.Token);

                _authProvider.NotifyUserAuthentication(result.Token);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro no login: {ex.Message}");
                return false;
            }
        }

        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("token");
            _http.DefaultRequestHeaders.Authorization = null;
            _authProvider.NotifyUserLogout();
        }

        public async Task<string?> GetTokenAsync()
        {
            return await _localStorage.GetItemAsync<string>("token");
        }
    }
}
