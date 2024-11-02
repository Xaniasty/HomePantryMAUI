using HomePantry.Structure.Views;

namespace HomePantry
{
    public partial class App : Application
    {
        public static string? lastLoginEmail { get; set; }
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
