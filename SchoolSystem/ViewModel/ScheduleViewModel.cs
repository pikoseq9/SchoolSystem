using SchoolSystem.Model;
using SchoolSystem.Repositories;
using SchoolSystem.ViewModel.BaseClass;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SchoolSystem.ViewModel
{
    public class ScheduleViewModel : BaseViewModel
    {
        private ObservableCollection<LessonDisplay>? _lessonsDisplay;
        private readonly ScheduleRepository _scheduleRepository;
        private readonly TeacherRepository _teacherRepository;
        private readonly SubjectRepository _subjectRepository;
        private readonly RoomRepository _roomRepository;
        private int _classId;

        public ObservableCollection<LessonDisplay>? LessonsDisplay
        {
            get => _lessonsDisplay;
            set
            {
                _lessonsDisplay = value;
                OnPropertyChanged(nameof(LessonsDisplay));
            }
        }

        public ScheduleViewModel()
        {
            _scheduleRepository = new ScheduleRepository();
            _teacherRepository = new TeacherRepository();
            _subjectRepository = new SubjectRepository();
            _roomRepository = new RoomRepository();

            _classId = 2; // Testowe ID klasy – do podmiany na dynamiczne ID po logowaniu

            LoadLessons();
        }

        private void LoadLessons()
        {
            try
            {
                var lessons = _scheduleRepository.GetLessonsByClassId(_classId);

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
                                "Piątek" => 4
                                
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
                                "15:00" => 7
                            
                            }
                        };
                    })
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy pobieraniu planu zajęć: {ex.Message}");
            }
        }
    }
}