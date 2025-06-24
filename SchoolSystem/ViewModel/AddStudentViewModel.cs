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
    public class AddStudentViewModel : BaseViewModel
    {
        private Student _student = new Student();

        private StudentRepository _studentRepository = new StudentRepository();
        private ClassRepository _classRepository = new ClassRepository();

        // Kolekcja klas do wyboru w UI
        public ObservableCollection<Class> AvailableClasses { get; } = new ObservableCollection<Class>();

        private Class _selectedClass;
        public Class SelectedClass
        {
            get => _selectedClass;
            set { _selectedClass = value; OnPropertyChanged(nameof(SelectedClass)); }
        }

        // Dane studenta
        public string Name
        {
            get => _student.Name;
            set { _student.Name = value; OnPropertyChanged(nameof(Name)); }
        }
        public string SurName
        {
            get => _student.SurName;
            set { _student.SurName = value; OnPropertyChanged(nameof(SurName)); }
        }
        public DateTime? DateOfBirth
        {
            get => _student.DateOfBirth;
            set { _student.DateOfBirth = value; OnPropertyChanged(nameof(DateOfBirth)); }
        }
        public string Gender
        {
            get => _student.Gender;
            set { _student.Gender = value; OnPropertyChanged(nameof(Gender)); }
        }
        public string PESEL
        {
            get => _student.PESEL;
            set { _student.PESEL = value; OnPropertyChanged(nameof(PESEL)); }
        }
        public string Login
        {
            get => _student.Login;
            set { _student.Login = value; OnPropertyChanged(nameof(Login)); }
        }
        public string Password
        {
            get => _student.Password;
            set { _student.Password = value; OnPropertyChanged(nameof(Password)); }
        }

        // Komendy
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public AddStudentViewModel()
        {
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);

            LoadClasses();
        }

        private void LoadClasses()
        {
            var classes = _classRepository.GetAllClasses();
            AvailableClasses.Clear();
            foreach (var c in classes)
                AvailableClasses.Add(c);

            // Opcjonalnie ustaw domyślnie pierwszą klasę
            if (AvailableClasses.Count > 0)
                SelectedClass = AvailableClasses[0];
        }

        private void Save()
        {
            // Walidacja pól, możesz rozszerzyć o inne
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
            if (string.IsNullOrEmpty(PESEL) || PESEL.Length != 11 || !PESEL.All(char.IsDigit))
            {
                MessageBox.Show("PESEL musi składać się z dokładnie 11 cyfr.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SelectedClass == null)
            {
                MessageBox.Show("Wybierz klasę.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Gender == null)
            {
                MessageBox.Show("Wybierz płeć.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrEmpty(Login))
            {
                MessageBox.Show("Login nie może być pusty.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (_studentRepository.StudentExistsByPESEL(PESEL))
            {
                MessageBox.Show("Uczeń z takim numerem PESEL już istnieje.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_studentRepository.StudentExistsByLogin(Login))
            {
                MessageBox.Show("Login jest już zajęty.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Hasło nie może być puste.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var hashedPassword = PasswordHelper.HashPassword(Password);
            _student.Password = hashedPassword;

            // Przypisujemy ID klasy wybranej przez użytkownika
            _student.ClassID = SelectedClass.Id;

            _studentRepository.AddStudent(_student);
            CloseRequested?.Invoke(this, true);
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(this, false);
        }

        public event EventHandler<bool> CloseRequested;
    }
}
