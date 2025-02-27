﻿using System.Diagnostics;
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
        HttpClientHandler handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

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

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Users/{userId}");
            Debug.WriteLine($"Response status for GetUserByIdAsync: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                var user = await response.Content.ReadFromJsonAsync<User>();
                Debug.WriteLine($"User fetched: {user?.Login}");
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
            Debug.WriteLine($"Błąd podczas pobierania danych użytkownika: {ex.Message}");
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

    public async Task<bool> CreateGranaryAsync(Granary granary)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Granary", granary);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Błąd przy tworzeniu magazynu: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteGranaryAsync(int granaryId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Granary/{granaryId}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Błąd przy usuwaniu magazynu: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> CreateShoplistAsync(Shoplist shoplist)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/Shoplist", shoplist);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Błąd przy tworzeniu listy zakupów: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteShoplistAsync(int shoplistId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Shoplist/{shoplistId}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Błąd przy usuwaniu listy zakupów: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateGranaryAsync(Granary granary)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Granary/{granary.Id}", granary);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating granary: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateShoplistAsync(Shoplist shoplist)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Shoplist/{shoplist.Id}", shoplist);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Błąd przy edytowaniu listy zakupów: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteAllGranariesForUserAsync(int userId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Granary/user/{userId}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Error deleting all granaries: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteAllShoplistsForUserAsync(int userId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Shoplist/user/{userId}");
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Debug.WriteLine($"Error deleting all shoplists: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> AddProductToGranaryAsync(ProductsInGranary product, int granaryId)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/ProductsInGranary", product);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in AddProductToGranaryAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateProductInGranaryAsync(ProductsInGranary product)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/ProductsInGranary/{product.ProductId}", product);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in UpdateProductInGranaryAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteProductFromGranaryAsync(int productId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/ProductsInGranary/{productId}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in DeleteProductFromGranaryAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<List<ProductsInGranary>> GetProductsInGranaryAsync(int granaryId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/ProductsInGranary/{granaryId}");
            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<List<ProductsInGranary>>();
                return products ?? new List<ProductsInGranary>();
            }
            return new List<ProductsInGranary>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in GetProductsInGranaryAsync: {ex.Message}");
            return new List<ProductsInGranary>();
        }
    }

    // Metody dla Shoplist

    public async Task<bool> AddProductToShoplistAsync(ProductsInShoplist product, int shoplistId)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"api/ProductsInShoplist", product);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in AddProductToShoplistAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateProductInShoplistAsync(ProductsInShoplist product)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"api/ProductsInShoplist/{product.ProductId}", product);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in UpdateProductInShoplistAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteProductFromShoplistAsync(int productId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/ProductsInShoplist/{productId}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in DeleteProductFromShoplistAsync: {ex.Message}");
            return false;
        }
    }

    public async Task<List<ProductsInShoplist>> GetProductsInShoplistAsync(int shoplistId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/ProductsInShoplist/{shoplistId}");
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<List<ProductsInShoplist>>()
                : new List<ProductsInShoplist>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in GetProductsInShoplistAsync: {ex.Message}");
            return new List<ProductsInShoplist>();
        }
    }

    public async Task<Granary?> CreateGranaryFromShoplistAsync(int shoplistId, int userId)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/Granary/CreateFromShoplist/user/{userId}/{shoplistId}", null);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<Granary>()
                : null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in CreateGranaryFromShoplistAsync: {ex.Message}");
            return null;
        }
    }

    public async Task<Shoplist?> CreateShoplistFromGranaryAsync(int granaryId, int userId)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/Shoplist/CreateShoplistFromGranary/user/{userId}/{granaryId}", null);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadFromJsonAsync<Shoplist>()
                : null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in CreateShoplistFromGranaryAsync: {ex.Message}");
            return null;
        }
    }



}
