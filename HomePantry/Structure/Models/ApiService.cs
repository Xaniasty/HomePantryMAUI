using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HomePantry.Models;
using HomePantry.Structure.Models;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };

        _httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://10.0.2.2:7147/")
        };
    }

    public async Task<User?> LoginAsync(string emailOrLogin, string password)
    {
        var loginRequest = new LoginRequest
        {
            EmailOrLogin = emailOrLogin,
            Password = password
        };

        Debug.WriteLine($"Attempting to log in with EmailOrLogin: {emailOrLogin}");

        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Users/login", loginRequest);
            Debug.WriteLine($"Response status: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<User>();
                return user;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error response: {errorContent}");
                return null;
            }
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Wystąpił błąd podczas łączenia z API: {ex.Message}");
            return null;
        }
    }


    public async Task<bool> CreateUserAsync(UserRegistrationRequest user)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Users", user);
            Debug.WriteLine($"Response status: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                return true; 
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error response: {errorContent}");
                return false; 
            }
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Wystąpił błąd podczas łączenia z API: {ex.Message}");
            return false; 
        }
    }


    public async Task<List<Granary>> GetGranariesForUserAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Granary/{userId}");
            Debug.WriteLine($"Response status: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                var granaries = await response.Content.ReadFromJsonAsync<List<Granary>>();
                return granaries;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error response: {errorContent}");
                return new List<Granary>();
            }
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Wystąpił błąd podczas łączenia z API: {ex.Message}");
            return new List<Granary>();
        }
    }


    public async Task<List<Shoplist>> GetShoplistsForUserAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Shoplist/{userId}");
            Debug.WriteLine($"Response status: {response.StatusCode}");
            

            if (response.IsSuccessStatusCode)
            {
                var shoplists = await response.Content.ReadFromJsonAsync<List<Shoplist>>();
                return shoplists;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Error response: {errorContent}");
                return new List<Shoplist>();
            }
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Wystąpił błąd podczas łączenia z API: {ex.Message}");
            return new List<Shoplist>();
        }
    }


}
