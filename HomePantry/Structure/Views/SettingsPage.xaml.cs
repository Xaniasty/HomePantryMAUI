using Microsoft.Maui.Controls;
using HomePantry.Structure.ViewModels;

namespace HomePantry.Structure.Views
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
            BindingContext = new SettingsViewModel();
        }
    }
}