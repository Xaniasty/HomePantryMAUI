using HomePantry.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace HomePantry;

public class ProfileViewModel : INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private User _user;

    public ProfileViewModel(ApiService apiService)
    {
        _apiService = apiService;
    }

    public string UserName => _user?.Login ?? "N/A";
    public string UserEmail => _user?.Email ?? "N/A";
    public int GranariesCount { get; private set; }
    public int ShoplistsCount { get; private set; }
    public int TotalProductsCount { get; private set; }

    public async Task LoadUserDataAsync()
    {
        Debug.WriteLine("LoadUserDataAsync started");
        Debug.WriteLine($"App.user: {App.user?.Login}, ID: {App.user?.Id}");
        Debug.WriteLine($"App.CurrentUserId: {App.CurrentUserId}");
        int? userId = App.CurrentUserId;

        if (userId.HasValue)
        {
            Debug.WriteLine($"Current User ID: {userId.Value}");
            _user = await _apiService.GetUserByIdAsync(userId.Value);

            if (_user != null)
            {
                Debug.WriteLine($"User loaded: {_user.Login}, {_user.Email}");
                OnPropertyChanged(nameof(UserName));
                OnPropertyChanged(nameof(UserEmail));

                await LoadGranariesCountAsync();
                await LoadShoplistsCountAsync();
                await LoadTotalProductsCountAsync();
            }
            else
            {
                Debug.WriteLine("User not found.");
            }
        }
        else
        {
            Debug.WriteLine("User ID is null.");
        }
    }

    private async Task LoadGranariesCountAsync()
    {
        if (_user != null)
        {
            Debug.WriteLine($"Loading granaries for user: {_user.Id}");
            var granaries = await _apiService.GetGranariesForUserAsync(_user.Id.Value);
            GranariesCount = granaries.Count;

            Debug.WriteLine($"Granaries count: {GranariesCount}");
            OnPropertyChanged(nameof(GranariesCount));
        }
    }

    private async Task LoadShoplistsCountAsync()
    {
        if (_user != null)
        {
            var shoplists = await _apiService.GetShoplistsForUserAsync(_user.Id.Value);
            ShoplistsCount = shoplists.Count;
            OnPropertyChanged(nameof(ShoplistsCount));
        }
    }

    private async Task LoadTotalProductsCountAsync()
    {
        if (_user != null)
        {
            var granaries = await _apiService.GetGranariesForUserAsync(_user.Id.Value);

            int totalProducts = 0;
            foreach (var granary in granaries)
            {
                var products = await _apiService.GetProductsInGranaryAsync(granary.Id);
                totalProducts += products.Count;
            }

            TotalProductsCount = totalProducts;
            OnPropertyChanged(nameof(TotalProductsCount));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
