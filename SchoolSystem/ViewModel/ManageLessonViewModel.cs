using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.View;
using SchoolSystem.ViewModel.BaseClass;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;

namespace SchoolSystem.ViewModel
{
    public class ManageLessonViewModel : BaseViewModel
    {
        private readonly ScheduleRepository _scheduleRepo = new();
        private readonly ClassRepository _classRepo = new();

        public ObservableCollection<Class> Classes { get; set; }
        public ObservableCollection<LessonDisplay> LessonsDisplay { get; set; }

        private Class _selectedClass;
        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                _selectedClass = value;
                OnPropertyChanged(nameof(SelectedClass));
                LoadLessons(); // załaduj plan po zmianie klasy
            }
        }

        public ICommand AddLessonCommand { get; }
        public ICommand DeleteLessonCommand { get; }

        public ManageLessonViewModel()
        {
            Classes = _classRepo.GetAllClasses();
            LessonsDisplay = new ObservableCollection<LessonDisplay>();

            AddLessonCommand = new RelayCommand(AddLesson);

            DeleteLessonCommand = new RelayCommand(param =>
            {
                if (param is int lessonId)
                    DeleteLesson(lessonId);
            });
        }

        private void LoadLessons()
        {
            LessonsDisplay.Clear();

            if (SelectedClass == null) return;

            var lessons = _scheduleRepo.GetLessonsByClassId(SelectedClass.Id);
            foreach (var lesson in lessons)
            {
                LessonsDisplay.Add(new LessonDisplay(lesson)
                {
                    DeleteCommand = new RelayCommand(() => DeleteLesson(lesson.Id))
                });
            }


        }

        private void AddLesson()
        {
            if (SelectedClass == null)
            {
                MessageBox.Show("Najpierw wybierz klasę!");
                return;
            }

            var window = new AddLessonWindow(0, SelectedClass.Id);

            window.ShowDialog();
            LoadLessons(); // odśwież plan
        }

        private void DeleteLesson(int lessonId)
        {
            if (MessageBox.Show("Czy na pewno chcesz usunąć tę lekcję?", "Potwierdzenie", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                _scheduleRepo.DeleteLessonById(lessonId);
                LoadLessons();
            }
        }
    }
}