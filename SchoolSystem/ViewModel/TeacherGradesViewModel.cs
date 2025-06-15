using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.ViewModel.BaseClass;
using System;
using System.Collections.ObjectModel;

namespace SchoolSystem.ViewModel
{
    class TeacherGradesViewModel : BaseViewModel
    {
        private readonly Graderepository _graderepository;

        private ObservableCollection<Grade> _grades;
        public ObservableCollection<Grade> Grades
        {
            get => _grades;
            set
            {
                _grades = value;
                OnPropertyChanged(nameof(Grades));
            }
        }

        public TeacherGradesViewModel(int studentID)
        {
            _graderepository = new Graderepository();

            try
            {
                var gradesFromDb = _graderepository.GetAllGradesByStudentId(studentID);

                Grades = gradesFromDb ?? new ObservableCollection<Grade>();
                Console.WriteLine($"[DEBUG] Załadowano {Grades.Count} ocen dla ucznia ID {studentID}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Błąd podczas ładowania ocen: {ex.Message}");
                Grades = new ObservableCollection<Grade>(); // pusty fallback
            }
        }
    }
}
