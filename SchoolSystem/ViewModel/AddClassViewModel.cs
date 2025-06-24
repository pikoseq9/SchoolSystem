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
    public class AddClassViewModel : BaseViewModel
    {
        private readonly ClassRepository _classRepository = new ClassRepository();
        private readonly TeacherRepository _teacherRepository = new TeacherRepository();

        private Class _class = new Class();

        public string Code
        {
            get => _class.Code;
            set
            {
                _class.Code = value;
                OnPropertyChanged(nameof(Code));
            }
        }

        public ObservableCollection<Teacher> AvailableTeachers { get; } = new ObservableCollection<Teacher>();

        private Teacher _selectedTeacher;
        public Teacher SelectedTeacher
        {
            get => _selectedTeacher;
            set
            {
                _selectedTeacher = value;
                OnPropertyChanged(nameof(SelectedTeacher));
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event EventHandler<bool> CloseRequested;

        public AddClassViewModel()
        {
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);

            LoadTeachers();
        }

        private void LoadTeachers()
        {
            var teachers = _teacherRepository.GetAllTeachers();
            AvailableTeachers.Clear();
            foreach (var teacher in teachers)
                AvailableTeachers.Add(teacher);

            if (AvailableTeachers.Count > 0)
                SelectedTeacher = AvailableTeachers[0];
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Code))
            {
                MessageBox.Show("Kod klasy nie może być pusty.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Code.Length < 2 || Code.Length > 10)
            {
                MessageBox.Show("Kod klasy musi mieć od 2 do 10 znaków.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_classRepository.ClassCodeExists(Code))
            {
                MessageBox.Show("Kod klasy już istnieje. Wprowadź unikalny kod.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedTeacher == null)
            {
                MessageBox.Show("Wybierz wychowawcę.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _class.ClassTeacherID = SelectedTeacher.Id;

            try
            {
                _classRepository.AddClass(_class);
                MessageBox.Show("Klasa została dodana pomyślnie.", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                CloseRequested?.Invoke(this, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Wystąpił błąd podczas zapisu: {ex.Message}", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(this, false);
        }
    }
}
