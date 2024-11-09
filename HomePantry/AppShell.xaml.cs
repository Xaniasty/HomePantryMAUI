using HomePantry.Structure.Views;
using Microsoft.Maui.Controls;

namespace HomePantry
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Rejestracja tras
            Routing.RegisterRoute(nameof(FormPage), typeof(FormPage));
            Routing.RegisterRoute(nameof(ContainerDetailsPage), typeof(ContainerDetailsPage));
            Routing.RegisterRoute(nameof(ProductDetailsPage), typeof(ProductDetailsPage)); // Dodanie ProductDetailsPage
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            App.lastLoginEmail = null;
            Application.Current.MainPage = ((App)Application.Current).CreateLoginPage();
        }
    }
}