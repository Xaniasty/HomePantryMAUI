using HomePantry.Models;
using HomePantry.Structure.ViewModels;
using System.Collections.ObjectModel;

namespace HomePantry.Structure.Views;

[QueryProperty(nameof(GranaryId), "granaryId")]
[QueryProperty(nameof(ShoplistId), "shoplistId")]
public partial class ContainerDetailsPage : ContentPage
{
    private readonly ApiService _apiService;

    public int GranaryId { get; set; }
    public int ShoplistId { get; set; }

    // Konstruktor bezparametrowy wymagany przez Shell
    public ContainerDetailsPage()
    {
        InitializeComponent();
        _apiService = new ApiService(); // Inicjalizacja ApiService
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Ustawienie BindingContext w zale¿noœci od parametrów granaryId lub shoplistId
        if (GranaryId != 0)
        {
            BindingContext = new ContainerDetailsViewModel(_apiService, granaryId: GranaryId);
        }
        else if (ShoplistId != 0)
        {
            BindingContext = new ContainerDetailsViewModel(_apiService, shoplistId: ShoplistId);
        }
    }
}