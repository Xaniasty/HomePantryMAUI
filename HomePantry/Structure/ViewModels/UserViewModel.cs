using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using HomePantry.Models;
using HomePantry;
using System.Diagnostics;
using System.Linq;
using HomePantry.Structure.Views;
using Newtonsoft.Json;


public class UserViewModel : INotifyPropertyChanged
{
    private ObservableCollection<Granary> _granaries;
    private ObservableCollection<Shoplist> _shoplists;
    private IEnumerable<IDisplayContainers> _currentItemsSource;
    private IDisplayContainers _selectedItem;
    private int? _userId;
    private readonly ApiService _apiService;
    private string _currentListTitle;


    public IDisplayContainers SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged();
        }
    }
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

    public enum ViewType
    {
        Granary,
        Shoplist,
        ToDoTasks
    }

    public ViewType CurrentViewType { get; set; }

    public ICommand ShowGranariesCommand { get; }
    public ICommand ShowShoplistCommand { get; }
    public ICommand ShowTasksCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteAllVisibleCommand { get; }

    public ICommand OpenContainerCommand { get; }

    public UserViewModel()
    {
        _apiService = new ApiService();
        UserId = App.user.Id;

        ShowGranariesCommand = new Command(() => LoadItems(ViewType.Granary));
        ShowShoplistCommand = new Command(() => LoadItems(ViewType.Shoplist));
        ShowTasksCommand = new Command(LoadTasks);
        AddCommand = new Command(AddItem);
        DeleteCommand = new Command<IDisplayContainers>(DeleteItem);
        EditCommand = new Command<IDisplayContainers>(EditItem);
        DeleteAllVisibleCommand = new Command(async () => await DeleteAllVisibleItems());
        OpenContainerCommand = new Command<IDisplayContainers>(OpenContainerDetails);

        CurrentViewType = ViewType.Granary;
        LoadItems(CurrentViewType);
    }

    public string CurrentListTitle
    {
        get => _currentListTitle;
        set
        {
            _currentListTitle = value;
            OnPropertyChanged(nameof(CurrentListTitle));
        }
    }

    private async void OpenContainerDetails(IDisplayContainers container)
    {
        if (container is Granary granary)
        {
            // Nawigacja do ContainerDetailsPage dla Granary
            await Shell.Current.GoToAsync($"{nameof(ContainerDetailsPage)}?granaryId={granary.Id}");
        }
        else if (container is Shoplist shoplist)
        {
            // Nawigacja do ContainerDetailsPage dla Shoplist
            await Shell.Current.GoToAsync($"{nameof(ContainerDetailsPage)}?shoplistId={shoplist.Id}");
        }
    }

    //private async void LoadItems(ViewType viewType)
    //{
    //    CurrentViewType = viewType;

    //    if (viewType == ViewType.Granary)
    //    {
    //        var granaries = await _apiService.GetGranariesForUserAsync(UserId.Value);
    //        GranariesItems = new ObservableCollection<Granary>(granaries);
    //        CurrentItemsSource = GranariesItems.Cast<IDisplayContainers>();
    //        CurrentListTitle = "Granaries";
    //    }
    //    else if (viewType == ViewType.Shoplist)
    //    {
    //        var shoplists = await _apiService.GetShoplistsForUserAsync(UserId.Value);
    //        ShoplistItems = new ObservableCollection<Shoplist>(shoplists);
    //        CurrentItemsSource = ShoplistItems.Cast<IDisplayContainers>();
    //        CurrentListTitle = "Shoplist";
    //    }
    //    else
    //    {
    //        CurrentListTitle = "ToDoList";
    //    }
    //}

    private async void LoadItems(ViewType viewType)
    {
        CurrentViewType = viewType;

        switch (viewType)
        {
            case ViewType.Granary:
                var granaries = await _apiService.GetGranariesForUserAsync(UserId.Value);
                GranariesItems = new ObservableCollection<Granary>(granaries);
                CurrentItemsSource = GranariesItems.Cast<IDisplayContainers>();
                CurrentListTitle = "Granaries"; // Aktualizacja tytułu
                break;

            case ViewType.Shoplist:
                var shoplists = await _apiService.GetShoplistsForUserAsync(UserId.Value);
                ShoplistItems = new ObservableCollection<Shoplist>(shoplists);
                CurrentItemsSource = ShoplistItems.Cast<IDisplayContainers>();
                CurrentListTitle = "Shoplists"; // Aktualizacja tytułu
                break;

            case ViewType.ToDoTasks:
                LoadTasks();
                CurrentListTitle = "Tasks"; // Aktualizacja tytułu
                break;
        }
    }


    private void LoadTasks()
    {
        CurrentViewType = ViewType.ToDoTasks;
        CurrentListTitle = "Tasks";

        CurrentItemsSource = new List<IDisplayContainers>
        {
            new Granary { GranaryName = "Zadanie 1", Opis = "Opis zadania 1" },
            new Granary { GranaryName = "Zadanie 2", Opis = "Opis zadania 2" },
            new Granary { GranaryName = "Zadanie 3", Opis = "Opis zadania 3" },
        };
    }

    private async void AddItem()
    {
        if (CurrentViewType == ViewType.Granary)
        {
            var newGranary = new Granary { GranaryName = "Nowy Magazyn", UserId = UserId.Value, Opis = "Opis magazynu" };
            var success = await _apiService.CreateGranaryAsync(newGranary);
            if (success)
            {
                GranariesItems.Add(newGranary);
                CurrentItemsSource = GranariesItems.Cast<IDisplayContainers>();
                RefreshCurrentItemsSource();
            }
        }
        else if (CurrentViewType == ViewType.Shoplist)
        {
            var newShoplist = new Shoplist { ShoplistName = "Nowa Lista Zakupów", UserId = UserId.Value, Opis = "Opis listy" };
            var success = await _apiService.CreateShoplistAsync(newShoplist);
            if (success)
            {
                ShoplistItems.Add(newShoplist);
                CurrentItemsSource = ShoplistItems.Cast<IDisplayContainers>();
                RefreshCurrentItemsSource();
            }
        }
    }

    private async void DeleteItem(IDisplayContainers item)
    {
        if (item == null)
        {
            Debug.WriteLine("DeleteItem: item is null");
            return;
        }

        Debug.WriteLine($"Attempting to delete item: {item}");

        if (CurrentViewType == ViewType.Granary && item is Granary granary)
        {
            var success = await _apiService.DeleteGranaryAsync(granary.Id);
            if (success)
            {
                GranariesItems.Remove(granary);
                CurrentItemsSource = GranariesItems.Cast<IDisplayContainers>();
                Debug.WriteLine("Granary item deleted successfully");
            }
        }
        else if (CurrentViewType == ViewType.Shoplist && item is Shoplist shoplist)
        {
            var success = await _apiService.DeleteShoplistAsync(shoplist.Id);
            if (success)
            {
                ShoplistItems.Remove(shoplist);
                CurrentItemsSource = ShoplistItems.Cast<IDisplayContainers>();
                Debug.WriteLine("Shoplist item deleted successfully");
            }
        }
    }

    private async void EditItem(IDisplayContainers item)
    {
        if (item == null)
        {
            Debug.WriteLine("EditItem: item is null");
            return;
        }

        if (CurrentViewType == ViewType.Granary && item is Granary granary)
        {
            await Shell.Current.GoToAsync($"{nameof(FormPage)}?granary={Uri.EscapeDataString(JsonConvert.SerializeObject(granary))}");
        }
        else if (CurrentViewType == ViewType.Shoplist && item is Shoplist shoplist)
        {
            await Shell.Current.GoToAsync($"{nameof(FormPage)}?shoplist={Uri.EscapeDataString(JsonConvert.SerializeObject(shoplist))}");
        }
    }

    public async void RefreshCurrentItemsSource()
    {
        switch (CurrentViewType)
        {
            case ViewType.Granary:
                var granaries = await _apiService.GetGranariesForUserAsync(UserId.Value);
                GranariesItems = new ObservableCollection<Granary>(granaries);
                CurrentItemsSource = GranariesItems.Cast<IDisplayContainers>();
                break;

            case ViewType.Shoplist:
                var shoplists = await _apiService.GetShoplistsForUserAsync(UserId.Value);
                ShoplistItems = new ObservableCollection<Shoplist>(shoplists);
                CurrentItemsSource = ShoplistItems.Cast<IDisplayContainers>();
                break;

            case ViewType.ToDoTasks:
                LoadTasks();
                break;
        }
    }

    public async Task DeleteAllVisibleItems()
    {
        switch (CurrentViewType)
        {
            case ViewType.Granary:
                await _apiService.DeleteAllGranariesForUserAsync(UserId.Value);
                GranariesItems.Clear();
                CurrentItemsSource = GranariesItems.Cast<IDisplayContainers>();
                break;

            case ViewType.Shoplist:
                await _apiService.DeleteAllShoplistsForUserAsync(UserId.Value);
                ShoplistItems.Clear();
                CurrentItemsSource = ShoplistItems.Cast<IDisplayContainers>();
                break;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}