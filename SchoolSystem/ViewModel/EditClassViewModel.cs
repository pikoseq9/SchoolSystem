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
    public class EditClassViewModel : BaseViewModel
    {
        private Class _class;
        private ClassRepository _classRepository = new ClassRepository();
        private TeacherRepository _teacherRepository = new TeacherRepository();

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

        public string Code
        {
            get => _class.Code;
            set { _class.Code = value; OnPropertyChanged(nameof(Code)); }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public EditClassViewModel(Class schoolClass)
        {
            _class = schoolClass;

            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);

            LoadTeachers();

            if (_class.ClassTeacherID != 0)
            {
                SelectedTeacher = AvailableTeachers.FirstOrDefault(t => t.Id == _class.ClassTeacherID);
            }
        }

        private void LoadTeachers()
        {
            var teachers = _teacherRepository.GetAllTeachers();
            AvailableTeachers.Clear();
            foreach (var teacher in teachers)
                AvailableTeachers.Add(teacher);
        }

        private void Save()
        {
            if (string.IsNullOrWhiteSpace(Code))
            {
                MessageBox.Show("Kod klasy nie może być pusty.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (SelectedTeacher == null)
            {
                MessageBox.Show("Wybierz wychowawcę.", "Błąd walidacji", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _class.ClassTeacherID = SelectedTeacher.Id;

            CloseRequested?.Invoke(this, true);
        }

        private void Cancel()
        {
            CloseRequested?.Invoke(this, false);
        }

        public event EventHandler<bool> CloseRequested;
    }
}
