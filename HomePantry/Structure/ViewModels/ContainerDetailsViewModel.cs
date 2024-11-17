using HomePantry;
using HomePantry.Models;
using HomePantry.Structure.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Channels;
using System.Windows.Input;

public class ContainerDetailsViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    public ObservableCollection<ProductsInGranary> GranaryProducts { get; private set; } = new ObservableCollection<ProductsInGranary>();
    public ObservableCollection<ProductsInShoplist> ShoplistProducts { get; private set; } = new ObservableCollection<ProductsInShoplist>();

    private object _selectedItem;
    public object SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged(nameof(SelectedItem));
        }
    }

    public string ContainerTitle { get; set; }
    private int _granaryId;
    private int _shoplistId;

    public ICommand AddItemCommand { get; }
    public ICommand EditItemCommand { get; }
    public ICommand DeleteItemCommand { get; }
    public ICommand DeleteAllVisibleItemsCommand { get; }
    public ICommand ActionButtonCommand { get; }


    public string ActionButtonText => IsGranaryView ? "Stwórz Listę Zakupów" : "Stwórz Magazyn";

    public bool IsGranaryView => _granaryId != 0;
    public ObservableCollection<object> ContainerItems => IsGranaryView ? new ObservableCollection<object>(GranaryProducts) : new ObservableCollection<object>(ShoplistProducts);

    public ContainerDetailsViewModel(ApiService apiService, int granaryId = 0, int shoplistId = 0)
    {
        _apiService = apiService;
        _granaryId = granaryId;
        _shoplistId = shoplistId;

        AddItemCommand = new Command(AddItem);
        EditItemCommand = new Command<object>(EditItem);
        DeleteItemCommand = new Command<object>(DeleteItem);
        ActionButtonCommand = new Command(CreateNewContainer, () => _granaryId != 0 || _shoplistId != 0);

        if (IsGranaryView)
        {
            ContainerTitle = "Produkty w Magazynie";
            LoadGranaryProducts();
        }
        else
        {
            ContainerTitle = "Produkty na Liście Zakupów";
            LoadShoplistProducts();
        }

        OnPropertyChanged(nameof(ContainerItems));
    }

    private async void LoadGranaryProducts()
    {
        Debug.WriteLine($"Loading granary products");
        var products = await _apiService.GetProductsInGranaryAsync(_granaryId);
        GranaryProducts = new ObservableCollection<ProductsInGranary>(products);
        OnPropertyChanged(nameof(GranaryProducts));
        OnPropertyChanged(nameof(ContainerItems));
    }

    private async void LoadShoplistProducts()
    {
        Debug.WriteLine($"Loading shoplist products");
        var products = await _apiService.GetProductsInShoplistAsync(_shoplistId);
        ShoplistProducts = new ObservableCollection<ProductsInShoplist>(products);
        OnPropertyChanged(nameof(ShoplistProducts));
        OnPropertyChanged(nameof(ContainerItems));
    }

    private async void AddItem()
    {
        Debug.WriteLine("Wywołano AddItem()");

        string route;
        if (IsGranaryView)
        {
            Debug.WriteLine($"Dodawanie do Granary, GranaryId: {_granaryId}");
            var newProduct = new ProductsInGranary { ProductName = "Nowy Produkt", Quantity = 1, GranaryId = _granaryId };
            route = $"{nameof(ProductDetailsPage)}?product={Uri.EscapeDataString(JsonConvert.SerializeObject(newProduct))}";
        }
        else
        {
            Debug.WriteLine($"Dodawanie do Shoplist, ShoplistId: {_shoplistId}");
            var newProduct = new ProductsInShoplist { ProductName = "Nowy Produkt", Quantity = 1, ShoplistId = _shoplistId };

            route = $"{nameof(ProductDetailsPage)}?product={Uri.EscapeDataString(JsonConvert.SerializeObject(newProduct))}";
        }

        await Shell.Current.GoToAsync(route);
    }

    private async void EditItem(object item)
    {
        string route;

  
        if (IsGranaryView && item is ProductsInGranary productInGranary)
        {
            Debug.WriteLine($"Edytowanie produktu w Granary, ProductId: {productInGranary.ProductId}");

         
            route = $"{nameof(ProductDetailsPage)}?product={Uri.EscapeDataString(JsonConvert.SerializeObject(productInGranary))}";
        }
  
        else if (!IsGranaryView && item is ProductsInShoplist productInShoplist)
        {
            Debug.WriteLine($"Edytowanie produktu w Shoplist, ProductId: {productInShoplist.ProductId}");

      
            route = $"{nameof(ProductDetailsPage)}?product={Uri.EscapeDataString(JsonConvert.SerializeObject(productInShoplist))}";
        }
        else
        {
     
            Debug.WriteLine("Nie rozpoznano typu produktu do edycji.");
            return;
        }

   
        await Shell.Current.GoToAsync(route);
    }

    private async void CreateNewContainer()
    {
        if (!App.CurrentUserId.HasValue)
        {
            await Application.Current.MainPage.DisplayAlert("Błąd", "Nie można wykonać operacji. Brak ID użytkownika.", "OK");
            return;
        }

        int userId = App.CurrentUserId.Value; // Konwersja do int

        if (IsGranaryView)
        {
            Debug.WriteLine($"Tworzenie Shoplist z GranaryId: {_granaryId}");
            var newShoplist = await _apiService.CreateShoplistFromGranaryAsync(_granaryId, userId);
            if (newShoplist != null)
            {
                await Application.Current.MainPage.DisplayAlert("Sukces", "Lista zakupów została utworzona!", "OK");
            }
        }
        else
        {
            Debug.WriteLine($"Tworzenie Granary z ShoplistId: {_shoplistId}");
            var newGranary = await _apiService.CreateGranaryFromShoplistAsync(_shoplistId, userId);
            if (newGranary != null)
            {
                await Application.Current.MainPage.DisplayAlert("Sukces", "Magazyn został utworzony!", "OK");
            }
        }
    }

    private async void DeleteItem(object item)
    {
        Debug.WriteLine($"wywołano metode deleteitem: {item}");
        if (IsGranaryView && item is ProductsInGranary productInGranary)
        {
            var success = await _apiService.DeleteProductFromGranaryAsync(productInGranary.ProductId);
            if (success)
            {
                GranaryProducts.Remove(productInGranary);
                OnPropertyChanged(nameof(ContainerItems));
            }
        }
        else if (!IsGranaryView && item is ProductsInShoplist productInShoplist)
        {
            var success = await _apiService.DeleteProductFromShoplistAsync(productInShoplist.ProductId);
            if (success)
            {
                ShoplistProducts.Remove(productInShoplist);
                OnPropertyChanged(nameof(ContainerItems));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}