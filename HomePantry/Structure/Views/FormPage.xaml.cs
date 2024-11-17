namespace HomePantry.Structure.Views;
using HomePantry.Models;
using Newtonsoft.Json;

[QueryProperty(nameof(GranaryData), "granary")]
[QueryProperty(nameof(ShoplistData), "shoplist")]
public partial class FormPage : ContentPage
{
    private readonly ApiService _apiService; 

  
    public FormPage()
    {
        InitializeComponent();
        _apiService = new ApiService(); 
    }

    
    public FormPage(ApiService apiService, Granary granary = null, Shoplist shoplist = null)
    {
        InitializeComponent();
        _apiService = apiService; 
        BindingContext = new FormViewModel(_apiService, granary, shoplist);
    }

    public string GranaryData
    {
        set
        {
            var granary = JsonConvert.DeserializeObject<Granary>(Uri.UnescapeDataString(value));
            BindingContext = new FormViewModel(_apiService, granary, null); 
        }
    }

    public string ShoplistData
    {
        set
        {
            var shoplist = JsonConvert.DeserializeObject<Shoplist>(Uri.UnescapeDataString(value));
            BindingContext = new FormViewModel(_apiService, null, shoplist);
        }
    }
}
