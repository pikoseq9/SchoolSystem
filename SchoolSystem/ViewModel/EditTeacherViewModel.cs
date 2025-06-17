using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.ViewModel.BaseClass;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SchoolSystem.ViewModel
{
    public class EditTeacherViewModel : BaseViewModel
    {
        private Teacher _teacher;
        private TeacherRepository _teacherRepository;

        public string Name
        {
            get => _teacher.Name;
            set { _teacher.Name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string SurName
        {
            get => _teacher.SurName;
            set { _teacher.SurName = value; OnPropertyChanged(nameof(SurName)); }
        }

        public DateTime? DateOfBirth
        {
            get => _teacher.DateOfBirth;
            set { _teacher.DateOfBirth = value; OnPropertyChanged(nameof(DateOfBirth)); }
        }

        public string Gender
        {
            get => _teacher.Gender;
            set { _teacher.Gender = value; OnPropertyChanged(nameof(Gender)); }
        }

        public string PhoneNumber
        {
            get => _teacher.PhoneNumber;
            set { _teacher.PhoneNumber = value; OnPropertyChanged(nameof(PhoneNumber)); }
        }

        public string Login
        {
            get => _teacher.Login;
            set { _teacher.Login = value; OnPropertyChanged(nameof(Login)); }
        }

        public string Password
        {
            get => _teacher.Password;
            set { _teacher.Password = value; OnPropertyChanged(nameof(Password)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditTeacherViewModel(Teacher teacher)
        {
            _teacher = teacher;
            _teacherRepository = new TeacherRepository();  // tutaj tworzysz instancję repozytorium

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Name) || !Name.All(char.IsLetter))
            {
                MessageBox.Show("Imię musi być wypełnione i zawierać tylko litery.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(SurName) || !SurName.All(char.IsLetter))
            {
                MessageBox.Show("Nazwisko musi być wypełnione i zawierać tylko litery.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(PhoneNumber) || PhoneNumber.Length != 9 || !PhoneNumber.All(char.IsDigit))
            {
                MessageBox.Show("Numer telefonu musi składać się z dokładnie 9 cyfr.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(Login))
            {
                MessageBox.Show("Login nie może być pusty.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Hasło nie może być puste.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            CloseRequested?.Invoke(this, true);
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(this, false);
        }

        public event EventHandler<bool> CloseRequested;
    }

}
