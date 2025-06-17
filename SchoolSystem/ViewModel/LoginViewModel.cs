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
using SchoolSystem.Helpers;

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
                    OnPropertyChanged(nameof(typ_konta));
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
                OnPropertyChanged(nameof(Username));
                (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private string _password; // W ViewModelu lepiej przechowywać hasło w bezpieczny sposób, ale dla testów...
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                (LoginCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand LoginCommand { get; private set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
          
        }

        // Metoda wykonywana po kliknięciu przycisku Zaloguj
        private void ExecuteLogin(object _)
        {
            string passwordFromView = Password;

            var repositorys = new StudentRepository();
            var repositoryt = new TeacherRepository();

            try
            {
                var student = repositorys.GetStudentByLogin(Username?.Trim());

                if (student != null && PasswordHelper.VerifyPassword(passwordFromView?.Trim(), student.Password))
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
                var teacher = repositoryt.GetTeacherByLogin(Username?.Trim());

                if (teacher != null && PasswordHelper.VerifyPassword(passwordFromView?.Trim(), teacher.Password) && teacher.Login == "dyrektor")
                {
                    ErrorMessage = "Logowanie pomyślne!";
                    typ_konta = "dyrektor";
                    OnLoginSuccess?.Invoke();
                }
                else if(teacher != null && PasswordHelper.VerifyPassword(passwordFromView?.Trim(), teacher.Password))
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

        //Metoda sprawdzająca, czy przycisk Zaloguj powinien być aktywny
        private bool CanExecuteLogin(object _)
        {
            return !string.IsNullOrWhiteSpace(Username) &&
                   !string.IsNullOrWhiteSpace(Password);
        }



    }

   
}

