using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using HomePantry.Models;
using HomePantry.Structure.Models;
using HomePantry.Structure.Views;
using HomePantry;

public class CreateAccountViewModel : INotifyPropertyChanged
{
    private string _email;
    private string _login;
    private string _password;
    private string _repeatPassword;
    private string _errorMessage;

    private readonly ApiService _apiService;

    public CreateAccountViewModel()
    {
        _apiService = new ApiService();
        CreateUserCommand = new Command(async () => await CreateUser());
    }

    public string Email
    {
        get => _email;
        set { _email = value; OnPropertyChanged(); }
    }

    public string Login
    {
        get => _login;
        set { _login = value; OnPropertyChanged(); }
    }

    public string Password
    {
        get => _password;
        set { _password = value; OnPropertyChanged(); }
    }

    public string repeatPassword
    {
        get => _repeatPassword;
        set { _repeatPassword = value; OnPropertyChanged(); }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set { _errorMessage = value; OnPropertyChanged(); }
    }

    public ICommand CreateUserCommand { get; }

    private async Task CreateUser()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(repeatPassword))
        {
            ErrorMessage = "Wszystkie pola są wymagane.";
            return;
        }

        if(Password != repeatPassword)
        {
            ErrorMessage = "Hasła nie są identyczne";
            return;
        }

        var user = new UserRegistrationRequest
        {
            Email = Email,
            Login = Login,
            Password = Password
        };

        bool isSuccess = await _apiService.CreateUserAsync(user);
        if (isSuccess)
        {
            
            App.lastLoginEmail = Email;
            await Application.Current.MainPage.Navigation.PopToRootAsync();
        }
        else
        {
            ErrorMessage = "Nie udało się utworzyć użytkownika. Sprawdź dane i spróbuj ponownie.";
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
