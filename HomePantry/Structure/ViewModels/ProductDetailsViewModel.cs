using HomePantry.Models;
using HomePantry.Structure.Models.HomePantry.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HomePantry.Structure.ViewModels
{
    public class ProductDetailsViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private bool _isEditMode;

        // Zdefiniowanie modelu produktu dla Granary lub Shoplist
        public ProductsInGranary GranaryProduct { get; private set; }
        public ProductsInShoplist ShoplistProduct { get; private set; }
        public bool IsGranaryProduct => GranaryProduct != null;
        public ObservableCollection<string> ListaRodzajow { get; set; }
        public ICommand SaveCommand { get; }

        public ProductDetailsViewModel(ApiService apiService, IProduct product)
        {
            _apiService = apiService;
            ListaRodzajow = new ObservableCollection<string>
            {
                "Nabiał",
                "Warzywa",
                "Owoce",
                "Mięso",
                "Ryby",
                "Pieczywo",
                "Przetwory",
                "Kasze i Makarony",
                "Słodycze",
                "Przekąski",
                "Napój",
                "Mrożonki",
                "Konserwy",
                "Przyprawy",
                "Produkty zbożowe",
                "Sosy i Ketchup",
                "Olej i Oliwa",
        
                "Chemia gospodarcza",
                "Środki czystości",
                "Kosmetyki",
                "Produkty higieniczne",
                "Artykuły papiernicze",
                "Artykuły dla zwierząt",
        
                "INNE"
            };

            Debug.WriteLine("ProductDetailsViewModel został utworzony.");
            if (product is ProductsInGranary granaryProduct)
            {
                GranaryProduct = granaryProduct;
                _isEditMode = granaryProduct.ProductId > 0;
                if (granaryProduct.DataZakupu == null)
                    GranaryProduct.DataZakupu = DateOnly.FromDateTime(DateTime.Today);
            }
            else if (product is ProductsInShoplist shoplistProduct)
            {
                ShoplistProduct = shoplistProduct;
                _isEditMode = shoplistProduct.ProductId > 0;
                if (shoplistProduct.DataZakupu == null)
                    ShoplistProduct.DataZakupu = DateOnly.FromDateTime(DateTime.Today);
            }

            // Inicjalizacja komendy zapisu
            SaveCommand = new Command(async () => await SaveProductAsync());
        }

        private async Task SaveProductAsync()
        {
            bool success = false;

            Debug.WriteLine("Wywołana metoda saveProductAsync");
            if (IsGranaryProduct && GranaryProduct != null)
            {
                success = _isEditMode
                    ? await _apiService.UpdateProductInGranaryAsync(GranaryProduct)
                    : await _apiService.AddProductToGranaryAsync(GranaryProduct, GranaryProduct.GranaryId);
            }
            else if (ShoplistProduct != null)
            {
                success = _isEditMode
                    ? await _apiService.UpdateProductInShoplistAsync(ShoplistProduct)
                    : await _apiService.AddProductToShoplistAsync(ShoplistProduct, ShoplistProduct.ShoplistId);
            }

            // Powrót do poprzedniej strony po zapisie
            if (success)
            {
                await Shell.Current.GoToAsync("..");
            }
        }

        private string _rodzaj;
        public string Rodzaj
        {
            get => _rodzaj;
            set
            {
                _rodzaj = value;
                OnPropertyChanged(nameof(Rodzaj));
            }
        }
        public string ProductName
        {
            get => IsGranaryProduct ? GranaryProduct?.ProductName : ShoplistProduct?.ProductName;
            set
            {
                if (IsGranaryProduct) GranaryProduct.ProductName = value;
                else ShoplistProduct.ProductName = value;
                OnPropertyChanged(nameof(ProductName));
            }
        }

        public int Quantity
        {
            get => IsGranaryProduct ? (GranaryProduct?.Quantity ?? 0) : (ShoplistProduct?.Quantity ?? 0);
            set
            {
                if (IsGranaryProduct) GranaryProduct.Quantity = value;
                else ShoplistProduct.Quantity = value;
                OnPropertyChanged(nameof(Quantity));
            }
        }

        public string Description
        {
            get => IsGranaryProduct ? GranaryProduct?.Description : ShoplistProduct?.Description;
            set
            {
                if (IsGranaryProduct) GranaryProduct.Description = value;
                else ShoplistProduct.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public decimal? Cena
        {
            get => IsGranaryProduct ? GranaryProduct?.Cena : ShoplistProduct?.Cena;
            set
            {
                if (IsGranaryProduct) GranaryProduct.Cena = value;
                else ShoplistProduct.Cena = value;
                OnPropertyChanged(nameof(Cena));
            }
        }

        public decimal? Weight
        {
            get => IsGranaryProduct ? GranaryProduct?.Weight : ShoplistProduct?.Weight;
            set
            {
                if (IsGranaryProduct) GranaryProduct.Weight = value;
                else ShoplistProduct.Weight = value;
                OnPropertyChanged(nameof(Weight));
            }
        }

        public bool IsLiquid
        {
            get => IsGranaryProduct ? GranaryProduct?.IsLiquid ?? false : ShoplistProduct?.IsLiquid ?? false;
            set
            {
                if (IsGranaryProduct) GranaryProduct.IsLiquid = value;
                else ShoplistProduct.IsLiquid = value;
                OnPropertyChanged(nameof(IsLiquid));
            }
        }

        public DateTime? DataZakupu
        {
            get => IsGranaryProduct ? GranaryProduct?.DataZakupu?.ToDateTime(TimeOnly.MinValue) : ShoplistProduct?.DataZakupu?.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (IsGranaryProduct)
                    GranaryProduct.DataZakupu = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
                else
                    ShoplistProduct.DataZakupu = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
                OnPropertyChanged(nameof(DataZakupu));
            }
        }

        public DateTime? DataWaznosci
        {
            get => IsGranaryProduct ? GranaryProduct?.DataWaznosci?.ToDateTime(TimeOnly.MinValue) : ShoplistProduct?.DataWaznosci?.ToDateTime(TimeOnly.MinValue);
            set
            {
                if (IsGranaryProduct)
                    GranaryProduct.DataWaznosci = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
                else
                    ShoplistProduct.DataWaznosci = value.HasValue ? DateOnly.FromDateTime(value.Value) : null;
                OnPropertyChanged(nameof(DataWaznosci));
            }
        }

        // Obsługa zdarzenia PropertyChanged dla powiadamiania o zmianach w widoku
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
