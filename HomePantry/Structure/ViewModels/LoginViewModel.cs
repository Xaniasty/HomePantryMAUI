using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using HomePantry.Structure.Views;


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
            var isSuccess = await _apiService.LoginAsync(EmailOrLogin, Password);
            if (isSuccess)
            {
                
                if (IsRemembered)
                {
                    Preferences.Set("IsRemembered", true);
                    Preferences.Set("EmailOrLogin", EmailOrLogin);
                    Preferences.Set("Password", Password);
                }
                else
                {
                    
                    Preferences.Set("IsRemembered", false);
                    Preferences.Set("EmailOrLogin", string.Empty);
                    Preferences.Set("Password", string.Empty);
                }

                await Application.Current.MainPage.Navigation.PushAsync(new HomePage());
            }
            else
            {
                ErrorMessage = "Błędne hasło lub login.";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
