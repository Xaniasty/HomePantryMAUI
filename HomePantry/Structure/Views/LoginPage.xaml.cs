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

            if (Preferences.Get("IsRemembered", false))
            {
                LoginEntry.Text = Preferences.Get("EmailOrLogin", string.Empty);
                PasswordEntry.Text = Preferences.Get("Password", string.Empty);
                RememberMeCheckBox.IsChecked = true;
            }
            else
            {
                LoginEntry.Text = string.Empty;
                PasswordEntry.Text = string.Empty;
                RememberMeCheckBox.IsChecked = false;
            }

        }

        private async void OnLoginSuccess()
        {
            // Ustaw now� stron� jako HomePage po pomy�lnym logowaniu
            await Navigation.PushAsync(new HomePage());
        }

        private async void OnCreateAccountTapped(object sender, EventArgs e)
        {
            
            await Navigation.PushAsync(new CreateAccountPage());
          
        }

        private async void OnInfoIconClicked(object sender, EventArgs e)
        {
           
            await Navigation.PushAsync(new AppInfoPage());
        }

        private async void OnPolicyClicked(object sender, EventArgs e)
        {
            string title = "Warunki u�ytkowania oraz Polityka Prywatno�ci";
            string message = "Og�lne Warunki U�ytkowania\r\n1. Wprowadzenie\r\n\r\nNiniejsze og�lne warunki u�ytkowania (zwane dalej \"Warunkami\") reguluj� zasady korzystania z aplikacji mobilnej \"HomePantry\" (zwanej dalej \"Aplikacj�\"). Korzystaj�c z Aplikacji, u�ytkownik (zwany dalej \"U�ytkownikiem\") akceptuje te Warunki.\r\n2. Opis Aplikacji\r\n\r\nAplikacja \"HomePantry\" umo�liwia U�ytkownikom zarz�dzanie domowymi zasobami, tworzenie list zakup�w oraz �ledzenie stanu produkt�w.\r\n3. Rejestracja i Konto\r\n\r\nU�ytkownik jest zobowi�zany do podania prawdziwych i kompletnych informacji podczas rejestracji. U�ytkownik ponosi odpowiedzialno�� za bezpiecze�stwo swojego konta i has�a.\r\n4. Ograniczenia\r\n\r\nU�ytkownik zobowi�zuje si� do nieu�ywania Aplikacji w celach niezgodnych z prawem, w tym do nieprzesy�ania tre�ci obra�liwych, wulgarnych lub naruszaj�cych prawo.\r\n5. Zmiany Warunk�w\r\n\r\nOperator Aplikacji zastrzega sobie prawo do zmiany niniejszych Warunk�w. Zmiany b�d� publikowane w Aplikacji, a U�ytkownik powinien regularnie sprawdza� aktualno�ci.\r\n6. Kontakt\r\n\r\nWszelkie pytania dotycz�ce niniejszych Warunk�w nale�y kierowa� na adres e-mail: [Tw�j adres e-mail].\r\nPolityka Prywatno�ci\r\n1. Wprowadzenie\r\n\r\nNiniejsza polityka prywatno�ci (zwana dalej \"Polityk�\") okre�la zasady zbierania, wykorzystywania i ochrony danych osobowych U�ytkownik�w Aplikacji \"HomePantry\".\r\n2. Zbieranie Danych\r\n\r\nAplikacja zbiera dane osobowe, takie jak: adres e-mail, login oraz has�o, kt�re s� niezb�dne do rejestracji i korzystania z Aplikacji.\r\n3. Wykorzystanie Danych\r\n\r\nZebrane dane s� wykorzystywane w celu:\r\nUmo�liwienia U�ytkownikowi korzystania z Aplikacji.\r\nKomunikacji z U�ytkownikiem (np. w przypadku resetu has�a).\r\nUlepszania Aplikacji i dostosowywania jej do potrzeb U�ytkownik�w.\r\n4. Przechowywanie Danych\r\n\r\nDane osobowe U�ytkownik�w s� przechowywane na zabezpieczonych serwerach i s� chronione przed nieautoryzowanym dost�pem.\r\n5. Ujawnianie Danych\r\n\r\nDane U�ytkownik�w nie s� sprzedawane ani udost�pniane osobom trzecim, chyba �e wymaga tego prawo lub w celu ochrony prawnych interes�w Operatora Aplikacji.\r\n6. Prawa U�ytkownik�w\r\n\r\nU�ytkownik ma prawo do dost�pu do swoich danych, ich poprawiania oraz ��dania ich usuni�cia.\r\n7. Zmiany Polityki\r\n\r\nOperator Aplikacji zastrzega sobie prawo do zmiany Polityki. Zmiany b�d� publikowane w Aplikacji, a U�ytkownik powinien regularnie sprawdza� aktualno�ci.\r\n8. Kontakt\r\n\r\nWszelkie pytania dotycz�ce niniejszej Polityki nale�y kierowa� na adres e-mail: homepantry@example.com.";
            string cancel = "Zamknij";

            await DisplayAlert(title, message, cancel);
        }

    }

}