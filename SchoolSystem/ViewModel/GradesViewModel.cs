using SchoolSystem.ViewModel.BaseClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.ViewModel.BaseClass;

namespace SchoolSystem.ViewModel
{
    public class GradesViewModel : BaseViewModel
    {
        private readonly Graderepository _graderepository;
        private ObservableCollection<Grade>? _grades;

        private int _currentStudentId; //testowe zanim nie ogarniemy przekazywania danych o zalogowanym userze

        public ObservableCollection<Grade> Grades
        {
            get { return _grades; }
            set
            {
                _grades = value;
                onPropertyChanged(nameof(Grades));
            }
        }

        public GradesViewModel()
        {
            _graderepository = new Graderepository();
            _grades = new ObservableCollection<Grade>();

            _currentStudentId = 1; //testowe zanim nie ogarniemy przekazywania danych o zalogowanym userze

            try
            {
                _grades.Clear();
                _grades = _graderepository.GetAllGradesByStudentId(_currentStudentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas ładowania ocen: {ex.Message}");
            }
        }
    }
}
