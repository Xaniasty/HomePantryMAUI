using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using HomePantry.Models;
using HomePantry;
using System.Diagnostics;
using System.Linq;

public class UserViewModel : INotifyPropertyChanged
{
    private ObservableCollection<Granary> _granaries;
    private ObservableCollection<Shoplist> _shoplists;
    private IEnumerable<IDisplayContainers> _currentItemsSource;

    private int? _userId;

    public int? UserId
    {
        get => _userId;
        set
        {
            _userId = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<IDisplayContainers> CurrentItemsSource
    {
        get => _currentItemsSource;
        set
        {
            _currentItemsSource = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Granary> GranariesItems
    {
        get => _granaries;
        set
        {
            _granaries = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<Shoplist> ShoplistItems
    {
        get => _shoplists;
        set
        {
            _shoplists = value;
            OnPropertyChanged();
        }
    }

    private readonly ApiService _apiService;

    public ICommand ShowGranariesCommand { get; }
    public ICommand ShowShoplistCommand { get; }
    public ICommand ShowTasksCommand { get; }

    public UserViewModel()
    {
        _apiService = new ApiService();
        UserId = App.user.Id;
        ShowGranariesCommand = new Command(LoadGranaries);
        ShowShoplistCommand = new Command(LoadShoplist);
        ShowTasksCommand = new Command(LoadTasks);

        LoadGranaries();
    }

    private async void LoadGranaries()
    {
        var granaries = await _apiService.GetGranariesForUserAsync(UserId.Value);
        GranariesItems = new ObservableCollection<Granary>(granaries);
        CurrentItemsSource = GranariesItems.Cast<IDisplayContainers>();
    }

    private async void LoadShoplist()
    {
        var shoplists = await _apiService.GetShoplistsForUserAsync(UserId.Value);
        ShoplistItems = new ObservableCollection<Shoplist>(shoplists);
        CurrentItemsSource = ShoplistItems.Cast<IDisplayContainers>(); 
    }

    private void LoadTasks()
    {
        CurrentItemsSource = new List<IDisplayContainers>
        {
            new Granary { GranaryName = "Zadanie 1", Opis = "Opis zadania 1" },
            new Granary { GranaryName = "Zadanie 2", Opis = "Opis zadania 2" },
            new Granary { GranaryName = "Zadanie 3", Opis = "Opis zadania 3" },
            new Granary { GranaryName = "Zadanie 4", Opis = "Opis zadania 4" },
            new Granary { GranaryName = "Zadanie 5", Opis = "Opis zadania 5" },
            new Granary { GranaryName = "Zadanie 6", Opis = "Opis zadania 6" },
            new Granary { GranaryName = "Zadanie 99999999999", Opis = "Opis zadania 99999999999" },
        };

        CurrentItemsSource = CurrentItemsSource.Cast<IDisplayContainers>();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
