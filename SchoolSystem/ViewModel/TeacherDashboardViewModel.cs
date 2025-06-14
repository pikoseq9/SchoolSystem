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

        public TeacherDashboardViewModel()
        {
            // Inicjalizacja komend przy użyciu RelayCommand
            // Zauważ, że konstruktor RelayCommand przyjmujący Action bez parametru jest używany
            NavigateToStudentListCommand = new RelayCommand(NavigateToStudentList);

            // Ustaw domyślny widok przy załadowaniu StudentDashboardView (np. Oceny)
            NavigateToStudentList();
        }

        // Metody wywoływane przez komendy, które zmieniają aktualny pod-ViewModel
        private void NavigateToStudentList()
        {
            CurrentTeacherDetailViewModel = new StudentListViewModel();

        }
    }
}
