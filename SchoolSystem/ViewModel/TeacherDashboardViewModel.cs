using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SchoolSystem.ViewModel.BaseClass;

namespace SchoolSystem.ViewModel
{
    internal class TeacherDashboardViewModel : BaseViewModel
    {
        private BaseViewModel _currentTeacherDetailViewModel;
        private readonly StudentListViewModel _studentListViewModel;
        public BaseViewModel CurrentTeacherDetailViewModel
        {
            get => _currentTeacherDetailViewModel;
            set
            {
                _currentTeacherDetailViewModel = value;
                OnPropertyChanged(); // Powiadom UI o zmianie
            }
        }

        // Komendy, które będą wywoływane przez przyciski w StudentDashboardView.xaml
        public ICommand NavigateToStudentListCommand { get; }
        public ICommand NavigateToTeacherRemarksComand { get; }
        public ICommand NavigateToTeacherGradesComand { get; }

        public TeacherDashboardViewModel()
        {
            // Inicjalizacja komend przy użyciu RelayCommand
            // Zauważ, że konstruktor RelayCommand przyjmujący Action bez parametru jest używany
            _studentListViewModel = new StudentListViewModel();

            NavigateToStudentListCommand = new RelayCommand(NavigateToStudentList);
            NavigateToTeacherRemarksComand = new RelayCommand(NavigateToTeacherRemarksEdit);
            NavigateToTeacherGradesComand = new RelayCommand(NavigateToTeacherGradesEdit);

            // Ustaw domyślny widok przy załadowaniu StudentDashboardView (np. Oceny)
            NavigateToStudentList();
        }

        // Metody wywoływane przez komendy, które zmieniają aktualny pod-ViewModel
        private void NavigateToStudentList()
        {
            //CurrentTeacherDetailViewModel = new StudentListViewModel();

            CurrentTeacherDetailViewModel = _studentListViewModel;

        }
        private void NavigateToTeacherRemarksEdit()
        {
            CurrentTeacherDetailViewModel = new TeacherRemarksViewModel();

        }

        private void NavigateToTeacherGradesEdit()
        {
            if (_studentListViewModel.SelectedStudent != null)
            {
                int studentId = _studentListViewModel.SelectedStudent.Id;
                CurrentTeacherDetailViewModel = new TeacherGradesViewModel(studentId);
                //CurrentTeacherDetailViewModel = new TeacherGradesViewModel();
            }
            else
            {
                System.Windows.MessageBox.Show("Najpierw wybierz ucznia z listy.", "Brak zaznaczenia", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }
        }

    }
}
