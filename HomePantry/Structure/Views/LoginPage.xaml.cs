using HomePantry.Structure.ViewModels;
using Microsoft.Maui.Controls;

namespace HomePantry.Structure.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }
    }
}