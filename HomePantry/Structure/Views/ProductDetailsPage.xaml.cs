using HomePantry.Models;
using HomePantry.Structure.Models.HomePantry.Models;
using HomePantry.Structure.ViewModels;
using Newtonsoft.Json;
using System.Diagnostics;

namespace HomePantry.Structure.Views
{
    [QueryProperty(nameof(ProductData), "product")]
    public partial class ProductDetailsPage : ContentPage
    {
        private readonly ApiService _apiService;

        public ProductDetailsPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        public string ProductData
        {
            set
            {
                try
                {
                    // Sprawdzanie typu produktu na podstawie pewnej w³aœciwoœci, np. GranaryId lub ShoplistId
                    var productJson = Uri.UnescapeDataString(value);

                    IProduct product;
                    if (productJson.Contains("\"GranaryId\"")) // Jeœli json zawiera "GranaryId", przyjmujemy, ¿e to produkt z Granary
                    {
                        product = JsonConvert.DeserializeObject<ProductsInGranary>(productJson);
                    }
                    else if (productJson.Contains("\"ShoplistId\"")) // Jeœli json zawiera "ShoplistId", przyjmujemy, ¿e to produkt z Shoplist
                    {
                        product = JsonConvert.DeserializeObject<ProductsInShoplist>(productJson);
                    }
                    else
                    {
                        throw new InvalidCastException("Unrecognized product type");
                    }

                    Debug.WriteLine($"Product data deserialized: {product?.ProductName}");
                    BindingContext = new ProductDetailsViewModel(_apiService, product);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Deserialization error: {ex.Message}");
                }
            }
        }
    }
}
