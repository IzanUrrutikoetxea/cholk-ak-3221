using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Blazored.LocalStorage;

namespace cholk_ak_3221.Services;

public class AuthService(HttpClient http, ILocalStorageService storage)
{
    private const string TokenKey = "cholk_jwt";
    private const string UserKey = "cholk_user";

    public AuthUser? CurrentUser { get; private set; }
    public bool IsAuthenticated => CurrentUser != null;

    public event Action? OnChange;

    public async Task InitializeAsync()
    {
        var token = await storage.GetItemAsStringAsync(TokenKey);
        var user = await storage.GetItemAsync<AuthUser>(UserKey);
        if (token != null && user != null)
        {
            CurrentUser = user;
            SetBearerToken(token);
        }
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var response = await http.PostAsJsonAsync("/auth/login", new { email, password });
        if (!response.IsSuccessStatusCode) return false;

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (result is null) return false;

        await PersistSession(result);
        return true;
    }

    public async Task<(bool Success, string? Error)> RegisterAsync(string email, string password, string displayName)
    {
        var response = await http.PostAsJsonAsync("/auth/register", new { email, password, displayName });
        if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            return (false, "Este email ya está registrado.");
        if (!response.IsSuccessStatusCode)
            return (false, "Error al registrar. Inténtalo de nuevo.");

        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (result is null) return (false, "Error inesperado.");

        await PersistSession(result);
        return (true, null);
    }

    public async Task LogoutAsync()
    {
        await storage.RemoveItemAsync(TokenKey);
        await storage.RemoveItemAsync(UserKey);
        CurrentUser = null;
        http.DefaultRequestHeaders.Authorization = null;
        OnChange?.Invoke();
    }

    private async Task PersistSession(AuthResponse result)
    {
        var user = new AuthUser(result.Email, result.DisplayName);
        await storage.SetItemAsStringAsync(TokenKey, result.Token);
        await storage.SetItemAsync(UserKey, user);
        CurrentUser = user;
        SetBearerToken(result.Token);
        OnChange?.Invoke();
    }

    private void SetBearerToken(string token) =>
        http.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
}

public record AuthUser(string Email, string DisplayName);

internal record AuthResponse(
    [property: JsonPropertyName("token")] string Token,
    [property: JsonPropertyName("email")] string Email,
    [property: JsonPropertyName("displayName")] string DisplayName);
