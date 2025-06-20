using System;
using System.ComponentModel;
using System.Windows.Input;
using SchoolSystem.View;
using SchoolSystem.View.Dashboards;
using SchoolSystem.ViewModel.BaseClass; // Make sure this namespace is correct

namespace SchoolSystem.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // Private field to hold login status
        private bool _isLoggedIn = false;

        // Property for the button's text, which will change based on login status
        private string _loginButtonText;
        public string LoginButtonText // Renamed from LoginText to be more descriptive for the button
        {
            get => _loginButtonText;
            set
            {
                _loginButtonText = value;
                OnPropertyChanged(nameof(LoginButtonText));
            }
        }

        // Property to expose and control the login status
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                // Update the button text whenever the login status changes
                LoginButtonText = _isLoggedIn ? "Wyloguj" : "Zaloguj";
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        // Commands (existing commands remain)
        public ICommand ShowLoginPageCommand { get; }
        public ICommand LoginStudentCommand { get; } // This command's logic is now handled by OnLoginSuccess callback

        // New command to handle the toggle logic for the button
        public ICommand ToggleLoginLogoutCommand { get; }


        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public MainViewModel()
        {
            // Initialize the login status to false (logged out) when the app starts
            IsLoggedIn = false;

            // Existing commands setup
            ShowLoginPageCommand = new RelayCommand(o => ShowLoginPage(), null);
            LoginStudentCommand = new RelayCommand(o => ShowPage(), null); // This remains but its primary use is now internal to ShowLoginPage/OnLoginSuccess flow

            // New command for the main button, handling both login (by showing page) and logout
            ToggleLoginLogoutCommand = new RelayCommand(o => ExecuteToggleLoginLogout(), null);

            // Set the initial view to the login page when the application starts
            ShowLoginPage();
        }

        /// <summary>
        /// This method determines whether to show the login page or log out.
        /// It's bound to the main "Zaloguj/Wyloguj" button.
        /// </summary>
        private void ExecuteToggleLoginLogout()
        {
            if (IsLoggedIn)
            {
                // If currently logged in, perform logout
                PerformLogout();
            }
            else
            {
                // If currently logged out, show the login page
                ShowLoginPage();
            }
        }

        /// <summary>
        /// Handles the logout logic.
        /// </summary>
        private void PerformLogout()
        {
            // Reset login status
            IsLoggedIn = false;
            // Optionally, clear any user-specific data from LoginViewModel or other places here

            // Navigate back to the login page
            ShowLoginPage();
        }

        public void ShowUserMenuPage()
        {
            // This method's logic remains unchanged based on your original request
            if (CurrentView is LoginView loginView && loginView.DataContext is LoginViewModel loginVM)
            {
                CurrentView = new UserMenu(loginVM);
            }
            else
            {
                CurrentView = new UserMenu(null); // lub nowy bez VM
            }
        }

        private void ShowPage()
        {
            // This method is now the callback for successful login from LoginViewModel
            if (CurrentView is LoginView loginView && loginView.DataContext is LoginViewModel loginVM)
            {
                // Set IsLoggedIn to true upon successful login
                IsLoggedIn = true;

                switch (loginVM.typ_konta)
                {
                    case "uczen":
                        CurrentView = new StudentDashboardView(loginVM.LoggedInUserId.Value);
                        break;

                    case "nauczyciel":
                        CurrentView = new TeacherDashboardView();
                        break;

                    case "dyrektor":
                        CurrentView = new PrincipalDashboardView();
                        break;

                    default:
                        // np. MessageBox.Show("Nieznany typ konta");
                        break;
                }
            }
        }

        private void ShowLoginPage()
        {
            var loginView = new LoginView();

            if (loginView.DataContext is LoginViewModel loginVM)
            {
                // Hook the ShowPage method as the callback for successful login
                loginVM.OnLoginSuccess = ShowPage;
            }

            CurrentView = loginView;
        }

        // Helper method for INotifyPropertyChanged
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}