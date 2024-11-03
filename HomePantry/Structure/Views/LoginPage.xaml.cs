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
            // Ustaw now¹ stronê jako HomePage po pomyœlnym logowaniu
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
            string title = "Warunki u¿ytkowania oraz Polityka Prywatnoœci";
            string message = "Ogólne Warunki U¿ytkowania\r\n1. Wprowadzenie\r\n\r\nNiniejsze ogólne warunki u¿ytkowania (zwane dalej \"Warunkami\") reguluj¹ zasady korzystania z aplikacji mobilnej \"HomePantry\" (zwanej dalej \"Aplikacj¹\"). Korzystaj¹c z Aplikacji, u¿ytkownik (zwany dalej \"U¿ytkownikiem\") akceptuje te Warunki.\r\n2. Opis Aplikacji\r\n\r\nAplikacja \"HomePantry\" umo¿liwia U¿ytkownikom zarz¹dzanie domowymi zasobami, tworzenie list zakupów oraz œledzenie stanu produktów.\r\n3. Rejestracja i Konto\r\n\r\nU¿ytkownik jest zobowi¹zany do podania prawdziwych i kompletnych informacji podczas rejestracji. U¿ytkownik ponosi odpowiedzialnoœæ za bezpieczeñstwo swojego konta i has³a.\r\n4. Ograniczenia\r\n\r\nU¿ytkownik zobowi¹zuje siê do nieu¿ywania Aplikacji w celach niezgodnych z prawem, w tym do nieprzesy³ania treœci obraŸliwych, wulgarnych lub naruszaj¹cych prawo.\r\n5. Zmiany Warunków\r\n\r\nOperator Aplikacji zastrzega sobie prawo do zmiany niniejszych Warunków. Zmiany bêd¹ publikowane w Aplikacji, a U¿ytkownik powinien regularnie sprawdzaæ aktualnoœci.\r\n6. Kontakt\r\n\r\nWszelkie pytania dotycz¹ce niniejszych Warunków nale¿y kierowaæ na adres e-mail: [Twój adres e-mail].\r\nPolityka Prywatnoœci\r\n1. Wprowadzenie\r\n\r\nNiniejsza polityka prywatnoœci (zwana dalej \"Polityk¹\") okreœla zasady zbierania, wykorzystywania i ochrony danych osobowych U¿ytkowników Aplikacji \"HomePantry\".\r\n2. Zbieranie Danych\r\n\r\nAplikacja zbiera dane osobowe, takie jak: adres e-mail, login oraz has³o, które s¹ niezbêdne do rejestracji i korzystania z Aplikacji.\r\n3. Wykorzystanie Danych\r\n\r\nZebrane dane s¹ wykorzystywane w celu:\r\nUmo¿liwienia U¿ytkownikowi korzystania z Aplikacji.\r\nKomunikacji z U¿ytkownikiem (np. w przypadku resetu has³a).\r\nUlepszania Aplikacji i dostosowywania jej do potrzeb U¿ytkowników.\r\n4. Przechowywanie Danych\r\n\r\nDane osobowe U¿ytkowników s¹ przechowywane na zabezpieczonych serwerach i s¹ chronione przed nieautoryzowanym dostêpem.\r\n5. Ujawnianie Danych\r\n\r\nDane U¿ytkowników nie s¹ sprzedawane ani udostêpniane osobom trzecim, chyba ¿e wymaga tego prawo lub w celu ochrony prawnych interesów Operatora Aplikacji.\r\n6. Prawa U¿ytkowników\r\n\r\nU¿ytkownik ma prawo do dostêpu do swoich danych, ich poprawiania oraz ¿¹dania ich usuniêcia.\r\n7. Zmiany Polityki\r\n\r\nOperator Aplikacji zastrzega sobie prawo do zmiany Polityki. Zmiany bêd¹ publikowane w Aplikacji, a U¿ytkownik powinien regularnie sprawdzaæ aktualnoœci.\r\n8. Kontakt\r\n\r\nWszelkie pytania dotycz¹ce niniejszej Polityki nale¿y kierowaæ na adres e-mail: homepantry@example.com.";
            string cancel = "Zamknij";

            await DisplayAlert(title, message, cancel);
        }

    }

}