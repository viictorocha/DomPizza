using Blazored.LocalStorage;
using DomPizza.App.Models;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace DomPizza.App.Services;

public class AuthService
{
    private readonly HttpClient _http;
    private readonly IConfiguration _config;
    private readonly ILocalStorageService _localStorage;
    private readonly IJSRuntime _jsRuntime;

    public AuthService(HttpClient http, IConfiguration config, ILocalStorageService localStorage, IJSRuntime jsRuntime)
    {
        _http = http;
        _config = config;
        _localStorage = localStorage;
        _jsRuntime = jsRuntime;
    }

    public async Task<bool> LoginAsync(LoginDTO dto)
    {
        try
        {
            var response = await _http.PostAsJsonAsync("https://localhost:7193/login", dto);

            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Login falhou: {msg}");
                return false;
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (result != null)
            {
                // salvar token no localStorage
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "token", result.Token);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro na requisição: {ex.Message}");
            return false;
        }
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
