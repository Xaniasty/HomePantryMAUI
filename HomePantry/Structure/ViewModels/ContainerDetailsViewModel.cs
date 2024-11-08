using HomePantry.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

public class ContainerDetailsViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    public ObservableCollection<ProductsInGranary> GranaryProducts { get; private set; } = new ObservableCollection<ProductsInGranary>();
    public ObservableCollection<ProductsInShoplist> ShoplistProducts { get; private set; } = new ObservableCollection<ProductsInShoplist>();
    public string ContainerTitle { get; set; }
    private int _granaryId;
    private int _shoplistId;

    public ICommand AddItemCommand { get; }
    public ICommand EditItemCommand { get; }
    public ICommand DeleteItemCommand { get; }

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
        var products = await _apiService.GetProductsInGranaryAsync(_granaryId);
        GranaryProducts = new ObservableCollection<ProductsInGranary>(products);
        OnPropertyChanged(nameof(GranaryProducts));
        OnPropertyChanged(nameof(ContainerItems));
    }

    private async void LoadShoplistProducts()
    {
        var products = await _apiService.GetProductsInShoplistAsync(_shoplistId);
        ShoplistProducts = new ObservableCollection<ProductsInShoplist>(products);
        OnPropertyChanged(nameof(ShoplistProducts));
        OnPropertyChanged(nameof(ContainerItems));
    }

    private async void AddItem()
    {
        Debug.WriteLine("Wywołano AddItem()");
        if (IsGranaryView)
        {
            Debug.WriteLine($"Dodawanie do Granary, GranaryId: {_granaryId}");
            var newProduct = new ProductsInGranary { ProductName = "Nowy Produkt", Quantity = 1, GranaryId = _granaryId };
            var success = await _apiService.AddProductToGranaryAsync(newProduct, _granaryId);
            if (success)
            {
                GranaryProducts.Add(newProduct);
                OnPropertyChanged(nameof(ContainerItems));
                
            }
        }
        else
        {
            Debug.WriteLine($"Dodawanie do Shoplist, ShoplistId: {_shoplistId}");
            var newProduct = new ProductsInShoplist { ProductName = "Nowy Produkt", Quantity = 1, ShoplistId = _shoplistId };
            var success = await _apiService.AddProductToShoplistAsync(newProduct, _shoplistId);
            if (success)
            {
                ShoplistProducts.Add(newProduct);
                OnPropertyChanged(nameof(ContainerItems));
            }
        }
    }

    private async void EditItem(object item)
    {
        if (IsGranaryView && item is ProductsInGranary productInGranary)
        {
            productInGranary.ProductName = "Zaktualizowany Produkt";
            var success = await _apiService.UpdateProductInGranaryAsync(productInGranary);
            if (success)
            {
                OnPropertyChanged(nameof(ContainerItems));
            }
        }
        else if (!IsGranaryView && item is ProductsInShoplist productInShoplist)
        {
            productInShoplist.ProductName = "Zaktualizowany Produkt";
            var success = await _apiService.UpdateProductInShoplistAsync(productInShoplist);
            if (success)
            {
                OnPropertyChanged(nameof(ContainerItems));
            }
        }
    }

    private async void DeleteItem(object item)
    {
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
