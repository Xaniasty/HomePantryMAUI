using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HomePantry.Structure.Views;
using Microsoft.Maui.Storage;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HomePantry.Structure.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _emailOrLogin;
        private string _password;
        private string _errorMessage;
        private bool _isRemembered;
        private bool _isAccepted;

        public string EmailOrLogin
        {
            get => _emailOrLogin;
            set
            {
                _emailOrLogin = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public bool IsRemembered
        {
            get => _isRemembered;
            set
            {
                _isRemembered = value;
                OnPropertyChanged();
            }
        }

        public bool IsAccepted
        {
            get => _isAccepted;
            set
            {
                _isAccepted = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        private readonly ApiService _apiService;

        public LoginViewModel()
        {
            _apiService = new ApiService();
            LoginCommand = new Command(async () => await ExecuteLoginCommand());

            IsRemembered = Preferences.Get("IsRemembered", false);
            IsAccepted = Preferences.Get("IsAccepted", false);

            if (IsRemembered)
            {
                EmailOrLogin = Preferences.Get("EmailOrLogin", string.Empty);
                Password = Preferences.Get("Password", string.Empty);
            }
        }

        private async Task ExecuteLoginCommand()
        {
            if (!IsAccepted)
            {
                ErrorMessage = "Musisz zaakceptować regulamin, aby się zalogować.";
                return;
            }

            ErrorMessage = "";

            Debug.WriteLine($"Attempting login with EmailOrLogin: {EmailOrLogin}");

            var user = await _apiService.LoginAsync(EmailOrLogin, Password);

            if (user != null)
            {
                Debug.WriteLine($"Login successful. User ID: {user.Id}, Login: {user.Login}");

                // Zapisanie stanu "IsAccepted"
                Preferences.Set("IsAccepted", IsAccepted);

                if (IsRemembered)
                {
                    Preferences.Set("IsRemembered", true);
                    Preferences.Set("EmailOrLogin", EmailOrLogin);
                    Preferences.Set("Password", Password);
                }
                else
                {
                    Preferences.Remove("IsRemembered");
                    Preferences.Remove("EmailOrLogin");
                    Preferences.Remove("Password");
                }

                // Ustawienie użytkownika w globalnym stanie aplikacji
                App.user = user;

                // Ustawienie ID użytkownika w globalnym stanie
                if (user.Id.HasValue)
                {
                    App.CurrentUserId = user.Id.Value;
                    Debug.WriteLine($"App.CurrentUserId set to: {App.CurrentUserId}");
                }
                else
                {
                    Debug.WriteLine("Warning: User ID is null.");
                }

                // Nawigacja do głównej części aplikacji
                (Application.Current as App)?.NavigateToAppShell();
            }
            else
            {
                ErrorMessage = "Błędne hasło lub login.";
                Debug.WriteLine("Login failed: Incorrect email/login or password.");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
