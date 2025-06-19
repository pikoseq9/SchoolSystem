using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.View;
using SchoolSystem.ViewModel.BaseClass;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace SchoolSystem.ViewModel
{
    public class TeacherGradesViewModel : BaseViewModel
    {
        private readonly Graderepository _graderepository;
        private readonly int _currentStudentId;

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

        private Grade _selectedGrade;
        public Grade SelectedGrade
        {
            get => _selectedGrade;
            set
            {
                _selectedGrade = value;
                OnPropertyChanged(nameof(SelectedGrade));

                // Bezpiecznie od razu po inicjalizacji
                if (DeleteGradeCommand is RelayCommand relay)
                    relay.RaiseCanExecuteChanged();
            }
        }

        public ICommand AddGradeCommand { get; }
        public ICommand DeleteGradeCommand { get; }


        public TeacherGradesViewModel(int studentID)
        {
            _currentStudentId = studentID;
            _graderepository = new Graderepository();

            AddGradeCommand = new RelayCommand(_ => AddGrade());
            DeleteGradeCommand = new RelayCommand(_ => DeleteSelectedGrade(), _ => SelectedGrade != null);

            try
            {
                var gradesFromDb = _graderepository.GetGradesWithSubjectNameByStudentId(studentID);
                Grades = gradesFromDb ?? new ObservableCollection<Grade>();
            }
            catch
            {
                Grades = new ObservableCollection<Grade>();
            }
        }


        private void AddGrade()
        {
            var vm = new AddGradeViewModel(_currentStudentId);

            var window = new AddGradeWindow(vm);

            vm.GradeConfirmed += grade =>
            {
                _graderepository.InsertGrade(grade);
                Grades = _graderepository.GetGradesWithSubjectNameByStudentId(grade.StudentID);

            };

            window.ShowDialog();
        }


        private void DeleteSelectedGrade()
        {
            if (SelectedGrade != null)
            {
                _graderepository.DeleteGrade(SelectedGrade.Id);
                Grades.Remove(SelectedGrade);
                SelectedGrade = null;
            }
        }
    }
}
