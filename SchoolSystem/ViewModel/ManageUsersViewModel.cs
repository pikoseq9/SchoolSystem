using SchoolSystem.Model;
using SchoolSystem.ViewModel.BaseClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using SchoolSystem.View;
using SchoolSystem.Repositories;

namespace SchoolSystem.ViewModel
{
    public class ManageUsersViewModel : BaseViewModel
    {
        private readonly StudentRepository _studentRepository = new StudentRepository();
        private readonly ClassRepository _classRepository = new ClassRepository();

        private ObservableCollection<Class> _classes;
        public ObservableCollection<Class> Classes
        {
            get => _classes;
            set
            {
                _classes = value;
                OnPropertyChanged(nameof(Classes));
            }
        }

        private ObservableCollection<Student> _students;
        public ObservableCollection<Student> Students
        {
            get => _students;
            set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        private Student _selectedStudent;
        public Student SelectedStudent
        {
            get => _selectedStudent;
            set
            {
                _selectedStudent = value;
                OnPropertyChanged(nameof(SelectedStudent));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand EditStudentCommand { get; }
        public ICommand DeleteStudentCommand { get; }
        public ICommand AddStudentCommand { get; }

        public ManageUsersViewModel()
        {
            Students = new ObservableCollection<Student>();

            EditStudentCommand = new RelayCommand(EditStudent, () => SelectedStudent != null);
            AddStudentCommand = new RelayCommand(AddStudent);
            DeleteStudentCommand = new RelayCommand(DeleteStudent, () => SelectedStudent != null);

            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                var studentsFromDb = _studentRepository.GetAllStudents();
                var classesFromDb = _classRepository.GetAllClasses();

                foreach (var student in studentsFromDb)
                {
                    student.Class = classesFromDb.FirstOrDefault(c => c.Id == student.ClassID);
                }

                Students = new ObservableCollection<Student>(studentsFromDb);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd ładowania uczniów: {ex.Message}");
            }
        }


        private void EditStudent()
        {
            if (SelectedStudent == null) return;

            var vm = new EditStudentViewModel(SelectedStudent);
            var view = new EditStudentView { DataContext = vm };

            var window = new Window
            {
                Title = "Edytuj ucznia",
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            vm.CloseRequested += (s, e) =>
            {
                if (e)
                {
                    _studentRepository.UpdateStudent(SelectedStudent);
                }
                window.DialogResult = e;
                window.Close();
            };

            window.ShowDialog();

            if (window.DialogResult == true)
            {
                LoadStudents();
            }
        }

        private void AddStudent()
        {
            var vm = new AddStudentViewModel();
            var view = new AddStudentView { DataContext = vm };

            var window = new Window
            {
                Title = "Dodaj ucznia",
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            vm.CloseRequested += (s, e) =>
            {
                if (e)
                {
                    LoadStudents();
                }
                window.DialogResult = e;
                window.Close();
            };

            window.ShowDialog();
        }

        private void DeleteStudent()
        {
            if (SelectedStudent == null) return;

            if (MessageBox.Show($"Usunąć ucznia {SelectedStudent.Name} {SelectedStudent.SurName}?",
                "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _studentRepository.DeleteStudent(SelectedStudent.Id);
                Students.Remove(SelectedStudent);
            }
        }

    }

}
