namespace HomePantry.Structure.Views;
using HomePantry.Models;
using Newtonsoft.Json;

[QueryProperty(nameof(GranaryData), "granary")]
[QueryProperty(nameof(ShoplistData), "shoplist")]
public partial class FormPage : ContentPage
{
    private readonly ApiService _apiService; // Dodaj pole _apiService

    // Konstruktor bezparametrowy
    public FormPage()
    {
        InitializeComponent();
        _apiService = new ApiService(); // Inicjalizuj _apiService
    }

    // Konstruktor z parametrami, który mo¿e byæ u¿ywany do testów lub inicjalizacji przez Dependency Injection
    public FormPage(ApiService apiService, Granary granary = null, Shoplist shoplist = null)
    {
        InitializeComponent();
        _apiService = apiService; // Ustaw _apiService na przekazany parametr
        BindingContext = new FormViewModel(_apiService, granary, shoplist);
    }

    public string GranaryData
    {
        set
        {
            var granary = JsonConvert.DeserializeObject<Granary>(Uri.UnescapeDataString(value));
            BindingContext = new FormViewModel(_apiService, granary, null); // U¿ycie _apiService z przekazanym granary
        }
    }

    public string ShoplistData
    {
        set
        {
            var shoplist = JsonConvert.DeserializeObject<Shoplist>(Uri.UnescapeDataString(value));
            BindingContext = new FormViewModel(_apiService, null, shoplist); // U¿ycie _apiService z przekazanym shoplist
        }
    }
}
