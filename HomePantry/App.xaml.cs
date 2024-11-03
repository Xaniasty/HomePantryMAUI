using HomePantry.Structure.Views;
using System.Diagnostics;

namespace HomePantry
{
    public partial class App : Application
    {
        public static string? lastLoginEmail { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = CreateLoginPage();
        }

        public NavigationPage CreateLoginPage()
        {
            var loginPage = new LoginPage();

            var primaryColor = (Color)Application.Current.Resources["PrimaryColor"];
            var secondaryColor = (Color)Application.Current.Resources["SecondaryColor"];

            var navigationPage = new NavigationPage(loginPage)
            {
                BarBackground = primaryColor, 
                BarTextColor = secondaryColor
            };
            navigationPage.BarBackgroundColor = primaryColor;
            navigationPage.BarTextColor = secondaryColor;
            


            return navigationPage;
        }

        public void NavigateToAppShell()
        {
            MainPage = new AppShell();
        }
    }
}
