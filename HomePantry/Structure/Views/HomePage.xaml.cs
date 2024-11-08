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

            // Od�wie�anie listy po powrocie do widoku
            if (BindingContext is UserViewModel viewModel)
            {
                viewModel.RefreshCurrentItemsSource(); // Wywo�aj metod� od�wie�ania w ViewModelu
            }
        }

        private async void OnMoreOptionsClicked(object sender, EventArgs e)
        {
            // Wy�wietl list� opcji w ma�ym okienku
            string action = await DisplayActionSheet("Wi�cej funkcji", "Anuluj", null,
                                                     "Usu� wszystko z widocznej listy",
                                                     "Inna opcja");

            // Obs�u� wybran� opcj�
            switch (action)
            {
                case "Usu� wszystko z widocznej listy":
                    await DeleteAllItems();
                    break;

                case "Inna opcja":
                    // Dodaj dodatkow� funkcj� tutaj, np. await SomeOtherFunction();
                    break;
            }
        }

        private async Task DeleteAllItems()
        {
            if (BindingContext is UserViewModel viewModel)
            {
                bool confirm = await DisplayAlert("Potwierdzenie", "Czy na pewno chcesz usun�� wszystkie elementy z widocznej listy?", "Tak", "Nie");
                if (confirm)
                {
                    await viewModel.DeleteAllVisibleItems();
                }
            }
        }




    }
}