using SchoolSystem.ViewModel.BaseClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SchoolSystem.Model;
using SchoolSystem.Repositories;

namespace SchoolSystem.ViewModel
{
    public class GradesViewModel : BaseViewModel
    {
        private readonly Graderepository _graderepository;
        private ObservableCollection<Grade>? _grades;

        public ObservableCollection<Grade> Grades
        {
            get { return _grades; }
            set
            {
                _grades = value;
                OnPropertyChanged(nameof(Grades));
            }
        }

        public GradesViewModel(int studentId)
        {
            _graderepository = new Graderepository();
            _grades = new ObservableCollection<Grade>();

            try
            {
                _grades.Clear();
                _grades = _graderepository.GetAllGradesByStudentId(studentId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas ładowania ocen: {ex.Message}");
            }
        }
        public GradesViewModel() : this(0)
        {
            _grades = new ObservableCollection<Grade>();
        }
    }
}
