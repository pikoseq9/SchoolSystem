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
    public class ManageClassViewModel : BaseViewModel
    {
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

        private Class _selectedClass;
        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ICommand AddClassCommand { get; }
        public ICommand EditClassCommand { get; }
        public ICommand DeleteClassCommand { get; }

        public ManageClassViewModel()
        {
            Classes = new ObservableCollection<Class>();

            AddClassCommand = new RelayCommand(AddClass);
            EditClassCommand = new RelayCommand(EditClass, () => SelectedClass != null);
            DeleteClassCommand = new RelayCommand(DeleteClass, () => SelectedClass != null);

            LoadClasses();
        }

        private void LoadClasses()
        {
            try
            {
                var classList = _classRepository.GetAllClasses();
                Classes = new ObservableCollection<Class>(classList);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Błąd ładowania klas: {ex.Message}");
            }
        }

        private void AddClass()
        {
            var vm = new AddClassViewModel();
            var view = new AddClassView { DataContext = vm };

            var window = new Window
            {
                Title = "Dodaj klasę",
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            vm.CloseRequested += (s, e) =>
            {
                if (e)
                {
                    LoadClasses();
                }
                window.DialogResult = e;
                window.Close();
            };

            window.ShowDialog();
        }

        private void EditClass()
        {
            if (SelectedClass == null) return;

            var vm = new EditClassViewModel(SelectedClass);
            var view = new EditClassView { DataContext = vm };

            var window = new Window
            {
                Title = "Edytuj klasę",
                Content = view,
                SizeToContent = SizeToContent.WidthAndHeight,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ResizeMode = ResizeMode.NoResize
            };

            vm.CloseRequested += (s, e) =>
            {
                if (e)
                {
                    _classRepository.UpdateClass(SelectedClass); 
                }
                window.DialogResult = e;
                window.Close();
            };

            window.ShowDialog();

            if (window.DialogResult == true)
            {
                LoadClasses();
            }
        }

        private void DeleteClass()
        {
            if (SelectedClass == null)
                return;

            if (MessageBox.Show($"Czy na pewno chcesz usunąć klasę {SelectedClass.Code}?",
                "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                bool deleted = _classRepository.DeleteClass(SelectedClass.Id);

                if (deleted)
                {
                    Classes.Remove(SelectedClass);
                }
                else
                {
                    MessageBox.Show("Nie można usunąć klasy, ponieważ są do niej przypisani uczniowie.",
                        "Błąd usuwania", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
