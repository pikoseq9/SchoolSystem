using System;
using System.ComponentModel;
using System.Windows.Input;
using SchoolSystem.View;
using SchoolSystem.View.Dashboards;
using SchoolSystem.ViewModel.BaseClass; 

namespace SchoolSystem.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isLoggedIn = false;

        private string _loginButtonText;
        public string LoginButtonText 
        {
            get => _loginButtonText;
            set
            {
                _loginButtonText = value;
                OnPropertyChanged(nameof(LoginButtonText));
            }
        }

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                LoginButtonText = _isLoggedIn ? "Wyloguj" : "Zaloguj";
                OnPropertyChanged(nameof(IsLoggedIn));
            }
        }

        public ICommand ShowLoginPageCommand { get; }
        public ICommand LoginStudentCommand { get; } 

   
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
 
            IsLoggedIn = false;


            ShowLoginPageCommand = new RelayCommand(o => ShowLoginPage(), null);
            LoginStudentCommand = new RelayCommand(o => ShowPage(), null); 

            
            ToggleLoginLogoutCommand = new RelayCommand(o => ExecuteToggleLoginLogout(), null);

            
            ShowLoginPage();
        }

     
        private void ExecuteToggleLoginLogout()
        {
            if (IsLoggedIn)
            {
                
                PerformLogout();
            }
            else
            {
                
                ShowLoginPage();
            }
        }

     
        private void PerformLogout()
        {
            
            IsLoggedIn = false;
            
            ShowLoginPage();
        }

        public void ShowUserMenuPage()
        {
           
            if (CurrentView is LoginView loginView && loginView.DataContext is LoginViewModel loginVM)
            {
                CurrentView = new UserMenu(loginVM);
            }
            else
            {
                CurrentView = new UserMenu(null); 
            }
        }

        private void ShowPage()
        {
            
            if (CurrentView is LoginView loginView && loginView.DataContext is LoginViewModel loginVM)
            {
                
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
                        
                        break;
                }
            }
        }

        private void ShowLoginPage()
        {
            var loginView = new LoginView();

            if (loginView.DataContext is LoginViewModel loginVM)
            {
                
                loginVM.OnLoginSuccess = ShowPage;
            }

            CurrentView = loginView;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}