using HomePantry.Structure.Views;

namespace HomePantry
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
