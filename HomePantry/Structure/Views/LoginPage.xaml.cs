using HomePantry.Structure.ViewModels;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Maui.Controls;

namespace HomePantry.Structure.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();

            if (!string.IsNullOrEmpty(App.lastLoginEmail))
            {
                LoginEntry.Text = App.lastLoginEmail;
            }


        }

        private async void OnCreateAccountTapped(object sender, EventArgs e)
        {
            
            await Navigation.PushAsync(new CreateAccountPage());
          
        }

        private async void OnInfoIconClicked(object sender, EventArgs e)
        {
           
            await Navigation.PushAsync(new AppInfoPage());
        }

    }

}