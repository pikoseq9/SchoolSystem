using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SchoolSystem.Repositories;
using SchoolSystem.ViewModel.BaseClass;
using SchoolSystem.Model;

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
        private readonly int _studentId;
        private int _classId;
        private  StudentRepository studentRepository = new StudentRepository();

        // Komendy, które będą wywoływane przez przyciski w StudentDashboardView.xaml
        public ICommand NavigateToGradesCommand { get; }
        public ICommand NavigateToRemarksCommand { get; }
        public ICommand NavigateToScheduleCommand { get; }

        public StudentDashboardViewModel(int studentId)
        {
            _studentId = studentId;
            Student student = studentRepository.GetStudentById(_studentId);
            _classId = student.ClassID;


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
            CurrentStudentDetailViewModel = new GradesViewModel(_studentId);
           
        }

        private void NavigateToRemarks()
        {
            CurrentStudentDetailViewModel = new RemarkViewModel(_studentId);
        }

        private void NavigateToSchedule()
        {
            CurrentStudentDetailViewModel = new ScheduleViewModel(_classId);
        }
    }

}
