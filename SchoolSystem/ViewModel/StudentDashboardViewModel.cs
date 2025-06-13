using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SchoolSystem.ViewModel
{
    internal class StudentDashboardViewModel : BaseViewModel
    {
        // Ta właściwość będzie przechowywać aktualnie wyświetlany pod-ViewModel
        // Jest bindowana do ContentControl w StudentDashboardView.xaml
        private BaseViewModel _currentStudentDetailViewModel;
        public BaseViewModel CurrentStudentDetailViewModel
        {
            get => _currentStudentDetailViewModel;
            set
            {
                _currentStudentDetailViewModel = value;
                OnPropertyChanged(); // Powiadom UI o zmianie
            }
        }

        // Komendy, które będą wywoływane przez przyciski w StudentDashboardView.xaml
        public ICommand NavigateToGradesCommand { get; }
        public ICommand NavigateToRemarksCommand { get; }
        public ICommand NavigateToScheduleCommand { get; }

        public StudentDashboardViewModel()
        {
            // Inicjalizacja komend przy użyciu RelayCommand
            // Zauważ, że konstruktor RelayCommand przyjmujący Action bez parametru jest używany
            NavigateToGradesCommand = new RelayCommand(NavigateToGrades);
            NavigateToRemarksCommand = new RelayCommand(NavigateToRemarks);
            NavigateToScheduleCommand = new RelayCommand(NavigateToSchedule);

            // Ustaw domyślny widok przy załadowaniu StudentDashboardView (np. Oceny)
            NavigateToGrades();
        }

        // Metody wywoływane przez komendy, które zmieniają aktualny pod-ViewModel
        private void NavigateToGrades()
        {
            CurrentStudentDetailViewModel = new GradesViewModel();
           
        }

        private void NavigateToRemarks()
        {
            CurrentStudentDetailViewModel = new RemarkViewModel();
        }

        private void NavigateToSchedule()
        {
            CurrentStudentDetailViewModel = new ScheduleViewModel();
        }
    }

}
