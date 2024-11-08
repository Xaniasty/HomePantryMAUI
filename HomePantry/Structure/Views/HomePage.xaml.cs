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

            if (BindingContext is UserViewModel viewModel)
            {
                viewModel.RefreshCurrentItemsSource(); 
            }
        }

        private async void OnMoreOptionsClicked(object sender, EventArgs e)
        {
            
            string action = await DisplayActionSheet("Wiêcej funkcji", "Anuluj", null,
                                                     "Usuñ wszystko z widocznej listy",
                                                     "Inna opcja");

            
            switch (action)
            {
                case "Usuñ wszystko z widocznej listy":
                    await DeleteAllItems();
                    break;

                case "Inna opcja":
                    
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