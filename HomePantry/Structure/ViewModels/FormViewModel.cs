using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HomePantry.Models;
using System.Threading.Tasks;
using System.Diagnostics;

public class FormViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private Granary _granary;
    private Shoplist _shoplist;

    public FormViewModel(ApiService apiService, Granary granary = null, Shoplist shoplist = null)
    {
        _apiService = apiService ?? new ApiService();
        _granary = granary;
        _shoplist = shoplist;

        SaveCommand = new Command(async () => await Save());
    }

    public string Name
    {
        get => _granary?.GranaryName ?? _shoplist?.ShoplistName ?? string.Empty;
        set
        {
            if (_granary != null && _granary.GranaryName != value)
            {
                _granary.GranaryName = value;
                OnPropertyChanged();
            }
            else if (_shoplist != null && _shoplist.ShoplistName != value)
            {
                _shoplist.ShoplistName = value;
                OnPropertyChanged();
            }
        }
    }

    public string Opis
    {
        get => _granary?.Opis ?? _shoplist?.Opis ?? string.Empty;
        set
        {
            if (_granary != null && _granary.Opis != value)
            {
                _granary.Opis = value;
                OnPropertyChanged();
            }
            else if (_shoplist != null && _shoplist.Opis != value)
            {
                _shoplist.Opis = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand SaveCommand { get; }

    private async Task Save()
    {
        if (_granary != null)
        {
            var result = await _apiService.UpdateGranaryAsync(_granary);
            Debug.WriteLine($"Update result for Granary: {result}");
        }
        else if (_shoplist != null)
        {
            var result = await _apiService.UpdateShoplistAsync(_shoplist);
            Debug.WriteLine($"Update result for Shoplist: {result}");
        }

        await Shell.Current.GoToAsync(".."); 
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}