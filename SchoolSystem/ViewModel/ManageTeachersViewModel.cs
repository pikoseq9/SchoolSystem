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
    public class ManageTeachersViewModel : BaseViewModel
    {
        private readonly TeacherRepository _teacherRepository = new TeacherRepository();


        private ObservableCollection<Teacher> _teachers;
        public ObservableCollection<Teacher> Teachers
        {
            get => _teachers;
            set
            {
                _teachers = value;
                OnPropertyChanged(nameof(Teachers));
            }
        }

        private Teacher _selectedTeacher;
        public Teacher SelectedTeacher
        {
            get => _selectedTeacher;
            set
            {
                _selectedTeacher = value;
                OnPropertyChanged(nameof(SelectedTeacher));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand EditTeacherCommand { get; }
        public ICommand DeleteTeacherCommand { get; }
        public ICommand AddTeacherCommand { get; }

        public ManageTeachersViewModel()
        {
            Teachers = new ObservableCollection<Teacher>();

            EditTeacherCommand = new RelayCommand(EditTeacher, () => SelectedTeacher != null);
            AddTeacherCommand = new RelayCommand(AddTeacher);
            DeleteTeacherCommand = new RelayCommand(DeleteTeacher, () => SelectedTeacher != null);

            LoadTeachers();
        }

        private void LoadTeachers()
        {
            try
            {
                var teachersFromDb = _teacherRepository.GetAllTeachers();

                Teachers = new ObservableCollection<Teacher>(teachersFromDb);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd ładowania nauczycieli: {ex.Message}");
            }
        }


        private void EditTeacher()
        {
            if (SelectedTeacher == null) return;

            var vm = new EditTeacherViewModel(SelectedTeacher);
            var view = new EditTeacherView { DataContext = vm };

            var window = new Window
            {
                Title = "Edytuj nauczyciela",
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            vm.CloseRequested += (s, e) =>
            {
                if (e)
                {
                    _teacherRepository.UpdateTeacher(SelectedTeacher);
                }
                window.DialogResult = e;
                window.Close();
            };

            window.ShowDialog();

            if (window.DialogResult == true)
            {
                LoadTeachers();
            }
        }

        private void AddTeacher()
        {
            var vm = new AddTeacherViewModel();
            var view = new AddTeacherView { DataContext = vm };

            var window = new Window
            {
                Title = "Dodaj nauczyciela",
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            vm.CloseRequested += (s, e) =>
            {
                if (e)
                {
                    LoadTeachers();
                }
                window.DialogResult = e;
                window.Close();
            };

            window.ShowDialog();
        }

        private void DeleteTeacher()
        {
            if (SelectedTeacher == null) return;

            if (MessageBox.Show($"Usunąć ucznia {SelectedTeacher.Name} {SelectedTeacher.SurName}?",
                "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                _teacherRepository.DeleteTeacher(SelectedTeacher.Id);
                Teachers.Remove(SelectedTeacher);
            }
        }

    }

}
