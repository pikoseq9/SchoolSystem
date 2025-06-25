using SchoolSystem.Helpers;
using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.ViewModel.BaseClass;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SchoolSystem.View;

namespace SchoolSystem.ViewModel
{
    public class TeacherScheduleViewModel : BaseViewModel
    {
        private ObservableCollection<LessonDisplay> _lessonsDisplay;
        private readonly ScheduleRepository _scheduleRepository;
        private readonly TeacherRepository _teacherRepository;
        private readonly SubjectRepository _subjectRepository;
        private readonly RoomRepository _roomRepository;
        private readonly ClassRepository _classRepository;

        public ICommand AddLessonCommand { get; }




        public ObservableCollection<LessonDisplay> LessonsDisplay
        {
            get => _lessonsDisplay;
            set
            {
                _lessonsDisplay = value;
                OnPropertyChanged(nameof(LessonsDisplay));
            }
        }

        public TeacherScheduleViewModel(int teacherId)
        {
            _scheduleRepository = new ScheduleRepository();
            _teacherRepository = new TeacherRepository();
            _subjectRepository = new SubjectRepository();
            _roomRepository = new RoomRepository();
            _classRepository = new ClassRepository();

            AddLessonCommand = new RelayCommand(OpenAddLessonWindow);

            LoadLessonsForTeacherClass(teacherId);
        }

        private void OpenAddLessonWindow()
        {
            var teacherId = Session.CurrentTeacher.Id;
            var teacherClass = _classRepository.GetAllClasses()
                                               .FirstOrDefault(k => k.ClassTeacherID == teacherId);
            if (teacherClass == null)
            {
                MessageBox.Show("Brak przypisanej klasy.");
                return;
            }

            var window = new AddLessonWindow
            {
                DataContext = new AddLessonViewModel(teacherId, teacherClass.Id)
            };
            window.ShowDialog();

            LoadLessonsForTeacherClass(teacherId);
        }

        private void DeleteLesson(int lessonId)
        {
            var result = MessageBox.Show("Czy na pewno chcesz usunąć tę lekcję?", "Potwierdzenie", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _scheduleRepository.DeleteLessonById(lessonId);
                LessonsDisplay = new ObservableCollection<LessonDisplay>(LessonsDisplay.Where(l => l.Id != lessonId));
            }
        }

        private void LoadLessonsForTeacherClass(int teacherId)
        {
            
            var teacherClass = _classRepository.GetAllClasses()
                                               .FirstOrDefault(k => k.ClassTeacherID == teacherId);

            if (teacherClass == null)
            {
                
                LessonsDisplay = new ObservableCollection<LessonDisplay>();
                return;
            }

            var classId = teacherClass.Id;

            var lessons = _scheduleRepository.GetLessonsByClassId(classId);


            LessonsDisplay = new ObservableCollection<LessonDisplay>(
                lessons.Select(lesson =>
                {
                    var teacher = _teacherRepository.GetTeacherById(lesson.TeacherID);
                    var subject = _subjectRepository.GetSubjectById(lesson.SubjectID);
                    var room = _roomRepository.GetRoomById(lesson.RoomID);

                    return new LessonDisplay
                    {
                        Id = lesson.Id,
                        DayOfWeek = lesson.DayOfWeek,
                        StartTime = lesson.StartTime,
                        Duration = lesson.Duration,
                        TeacherName = teacher != null ? $"{teacher.Name} {teacher.SurName}" : "Nieznany nauczyciel",
                        SubjectName = subject?.Name ?? "Nieznany przedmiot",
                        RoomName = room?.Number ?? "Nieznana sala",
                        GridColumn = lesson.DayOfWeek switch
                        {
                            "Poniedziałek" => 0,
                            "Wtorek" => 1,
                            "Środa" => 2,
                            "Czwartek" => 3,
                            "Piątek" => 4,
                            _ => 0
                        },
                        GridRow = lesson.StartTime switch
                        {
                            "08:00" => 0,
                            "09:00" => 1,
                            "10:00" => 2,
                            "11:00" => 3,
                            "12:00" => 4,
                            "13:00" => 5,
                            "14:00" => 6,
                            "15:00" => 7,
                            _ => 0
                        },
                        DeleteCommand = new RelayCommand(() => DeleteLesson(lesson.Id))
                    };
                })
            );
        }
    }
}
