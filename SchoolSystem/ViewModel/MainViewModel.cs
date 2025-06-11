using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolSystem.ViewModel
{
    using BaseClass;
    using SchoolSystem.View;
    using SchoolSystem.View.Dashboards;

    class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ShowLoginPageCommand { get; }

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentView)));
            }
        }

        public MainViewModel()
        {
            ShowLoginPageCommand = new RelayCommand(o => ShowLoginPage(), null);

            ShowLoginPage();
        }

        public void ShowUserDashboardPage()
        {
            if (CurrentView is LoginView loginView && loginView.DataContext is LoginViewModel loginVM)
            {
                switch (loginVM.typ_konta)
                {
                    case "uczen":
                        CurrentView = new StudentDashboardView(loginVM);
                        break;
                    case "nauczyciel":
                        CurrentView = new TeacherDashboardView(loginVM);
                        break;
                }
            }
        }

        private void ShowLoginPage()
        {
            var loginView = new LoginView();

            if (loginView.DataContext is LoginViewModel loginVM)
            {
                loginVM.OnLoginSuccess = ShowUserDashboardPage; 
            }

            CurrentView = loginView;
        }
    }


}
