using Blazored.LocalStorage;
using DomPizza.App.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace DomPizza.App.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private readonly ILocalStorageService _localStorage;

    public AuthService(HttpClient http, IConfiguration config, ILocalStorageService localStorage)
    {
        _http = http;
        _config = config;
        _localStorage = localStorage;
    }

    public async Task<bool> LoginAsync(LoginDTO dto)
    {
        var response = await _http.PostAsJsonAsync("login", dto);

        if (!response.IsSuccessStatusCode)
            return false;

        var content = await response.Content.ReadFromJsonAsync<JsonElement>();
        var token = content.GetProperty("token").GetString();

        if (token is null)
            return false;

        await _localStorage.SetItemAsync("authToken", token);
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return true;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        _http.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<string?> GetTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>("authToken");
    }
}
