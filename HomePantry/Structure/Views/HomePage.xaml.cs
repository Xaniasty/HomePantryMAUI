using Microsoft.Maui.Controls;
using HomePantry.Structure.ViewModels;
using HomePantry.Models;

namespace HomePantry.Structure.Views
{
    public partial class HomePage : ContentPage
    {
        private readonly UserViewModel _viewModel;
        

        public HomePage()
        {
            InitializeComponent();
            _viewModel = new UserViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Odœwie¿anie listy po powrocie do widoku
            if (BindingContext is UserViewModel viewModel)
            {
                viewModel.RefreshCurrentItemsSource(); // Wywo³aj metodê odœwie¿ania w ViewModelu
            }
        }

        private async void OnMoreOptionsClicked(object sender, EventArgs e)
        {
            // Wyœwietl listê opcji w ma³ym okienku
            string action = await DisplayActionSheet("Wiêcej funkcji", "Anuluj", null,
                                                     "Usuñ wszystko z widocznej listy",
                                                     "Inna opcja");

            // Obs³u¿ wybran¹ opcjê
            switch (action)
            {
                case "Usuñ wszystko z widocznej listy":
                    await DeleteAllItems();
                    break;

                case "Inna opcja":
                    // Dodaj dodatkow¹ funkcjê tutaj, np. await SomeOtherFunction();
                    break;
            }
        }

        private async Task DeleteAllItems()
        {
            if (BindingContext is UserViewModel viewModel)
            {
                bool confirm = await DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usun¹æ wszystkie elementy z widocznej listy?", "Tak", "Nie");
                if (confirm)
                {
                    await viewModel.DeleteAllVisibleItems();
                }
            }
        }




    }
}