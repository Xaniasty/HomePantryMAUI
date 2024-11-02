using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using HomePantry.Structure.Views;
using Microsoft.Maui.Controls;

namespace HomePantry.Structure.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _emailOrLogin;
        private string _password;
        private string _errorMessage;

        // Właściwość do bindowania emaila lub loginu
        public string EmailOrLogin
        {
            get => _emailOrLogin;
            set
            {
                _emailOrLogin = value;
                OnPropertyChanged();
            }
        }

        // Właściwość do bindowania hasła
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        // Właściwość do wyświetlania błędów
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        // Komenda do logowania
        public ICommand LoginCommand { get; }

        private readonly ApiService _apiService;

        public LoginViewModel()
        {
            _apiService = new ApiService();
            LoginCommand = new Command(async () => await ExecuteLoginCommand());
        }

        // Metoda logowania
        private async Task ExecuteLoginCommand()
        {
            ErrorMessage = ""; // Resetowanie błędów przed nową próbą
            var isSuccess = await _apiService.LoginAsync(EmailOrLogin, Password);
            if (isSuccess)
            {
                // Logika przejścia do strony głównej
                Application.Current.MainPage.Navigation.PushAsync(new HomePage());
            }
            else
            {
                ErrorMessage = "Błędne hasło lub login."; // Ustawienie komunikatu o błędzie
            }
        }

        // Implementacja INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
