using HomePantry.Models;
using HomePantry.Structure.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;

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
        Debug.WriteLine($"Granary id: {GranaryId} oraz shop id: {ShoplistId}");



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