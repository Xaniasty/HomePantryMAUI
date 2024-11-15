using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;

namespace HomePantry.Structure.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public List<string> PrimaryColors { get; } = new List<string> { "Red", "Blue", "Green", "Purple" };
        public List<string> SecondaryColors { get; } = new List<string> { "Yellow", "Orange", "Gray", "Pink" };

        private string _selectedPrimaryColor;
        public string SelectedPrimaryColor
        {
            get => _selectedPrimaryColor;
            set
            {
                _selectedPrimaryColor = value;
                OnPropertyChanged();
                Application.Current.Resources["PrimaryColor"] = Color.FromArgb(value);
            }
        }

        private string _selectedSecondaryColor;
        public string SelectedSecondaryColor
        {
            get => _selectedSecondaryColor;
            set
            {
                _selectedSecondaryColor = value;
                OnPropertyChanged();
                Application.Current.Resources["SecondaryColor"] = Color.FromArgb(value);
            }
        }

        private bool _isDarkModeEnabled;
        public bool IsDarkModeEnabled
        {
            get => _isDarkModeEnabled;
            set
            {
                _isDarkModeEnabled = value;
                OnPropertyChanged();
                Application.Current.UserAppTheme = value ? AppTheme.Dark : AppTheme.Light;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
