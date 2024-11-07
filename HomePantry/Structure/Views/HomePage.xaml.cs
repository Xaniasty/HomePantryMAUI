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


    }
}