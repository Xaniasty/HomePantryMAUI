using HomePantry.Structure.ViewModels;
using System.Diagnostics;

namespace HomePantry.Structure.Views;

public partial class ProfilePage : ContentPage
{
    private ProfileViewModel _viewModel;

    public ProfilePage()
    {
        InitializeComponent();
        _viewModel = new ProfileViewModel(new ApiService());
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Debug.WriteLine("ProfilePage OnAppearing");
        await _viewModel.LoadUserDataAsync();
    }
}

