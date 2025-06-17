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
    public class EditStudentViewModel : BaseViewModel
    {
        private Student _student;
        private StudentRepository _studentRepository;
        private ClassRepository _classRepository = new ClassRepository();

        public ObservableCollection<Class> AvailableClasses { get; } = new ObservableCollection<Class>();

        private Class _selectedClass;
        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
            }
        }
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

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditStudentViewModel(Student student)
        {
            _student = student;
            _studentRepository = new StudentRepository();  // tutaj tworzysz instancję repozytorium

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);

            LoadClasses();

            if (_student.ClassID != 0)
            {
                SelectedClass = AvailableClasses.FirstOrDefault(c => c.Id == _student.ClassID);
            }
        }

        private void LoadClasses()
        {
            var classes = _classRepository.GetAllClasses();
            AvailableClasses.Clear();
            foreach (var c in classes)
                AvailableClasses.Add(c);
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

            _student.ClassID = SelectedClass.Id;

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
