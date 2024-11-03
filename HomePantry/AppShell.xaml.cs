using HomePantry.Structure.Views;
using Microsoft.Maui.Controls;

namespace HomePantry
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            App.lastLoginEmail = null;
            Application.Current.MainPage = ((App)Application.Current).CreateLoginPage();
        }
    }
}