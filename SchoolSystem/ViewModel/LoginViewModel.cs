using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolSystem.ViewModel.BaseClass;
using System.Windows.Input;
using System.Diagnostics;
using SchoolSystem.Repositories;

namespace SchoolSystem.ViewModel
{
    public class LoginViewModel : SchoolSystem.ViewModel.BaseClass.BaseViewModel
    {
        private string _typ_konta;
        public string typ_konta
        {
            get => _typ_konta;
            set
            {
                if (_typ_konta != value)
                {
                    _typ_konta = value;
                    onPropertyChanged(nameof(typ_konta));
                }
            }
        }

        public Action OnLoginSuccess { get; set; }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                onPropertyChanged(nameof(Username));
            }
        }

        private string _password; // W ViewModelu lepiej przechowywać hasło w bezpieczny sposób, ale dla testów...
        public string Password // To pole będzie ustawiane przez "binding" z Code-Behind
        {
            get => _password;
            set
            {
                _password = value;
                onPropertyChanged(nameof(Password));
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                onPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand LoginCommand { get; private set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);

        }

        // Metoda wykonywana po kliknięciu przycisku Zaloguj
        private void ExecuteLogin(object parameter)
        {
            string passwordFromView = parameter as string;

            var repositorys = new StudentRepository();
            var repositoryt = new TeacherRepository();

            try
            {
                var student = repositorys.GetStudentByLogin(Username?.Trim(), passwordFromView?.Trim());

                if (student != null)
                {
                    ErrorMessage = "Logowanie pomyślne!";
                    typ_konta = "uczen";
                    OnLoginSuccess?.Invoke();
                }
                else
                {
                    ErrorMessage = "Błędny login lub hasło.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Błąd podczas logowania: {ex.Message}";
            }


            try
            {
                var student = repositoryt.GetTeacherByLogin(Username?.Trim(), passwordFromView?.Trim());

                if (student != null)
                {
                    ErrorMessage = "Logowanie pomyślne!";
                    typ_konta = "nauczyciel";
                    OnLoginSuccess?.Invoke();
                }
                else
                {
                    ErrorMessage = "Błędny login lub hasło.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Błąd podczas logowania: {ex.Message}";
            }
        }

        // Metoda sprawdzająca, czy przycisk Zaloguj powinien być aktywny
        private bool CanExecuteLogin(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Username) &&
           (parameter is string password && !string.IsNullOrWhiteSpace(password));
        }
    }

    // Pomocnicza klasa dla ICommand (jeśli jeszcze jej nie masz)
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}

