using SchoolSystem.Repositories;
using System.Windows.Input;

namespace SchoolSystem.Model
{
    public class LessonDisplay
    {
        public int Id { get; set; }
        public string DayOfWeek { get; set; }
        public string StartTime { get; set; }
        public int Duration { get; set; }

        public string SubjectName { get; set; }
        public string TeacherName { get; set; }
        public string RoomName { get; set; }

        public int GridRow { get; set; }
        public int GridColumn { get; set; }
        public ICommand DeleteCommand { get; set; }
        public LessonDisplay() { }

        public LessonDisplay(Lesson lesson)
        {
            Id = lesson.Id;
            DayOfWeek = lesson.DayOfWeek;
            StartTime = lesson.StartTime;
            Duration = lesson.Duration;

            var subject = new SubjectRepository().GetSubjectById(lesson.SubjectID);
            SubjectName = subject?.Name ?? "Nieznany przedmiot";

            var teacher = new TeacherRepository().GetTeacherById(lesson.TeacherID);
            TeacherName = teacher?.FullName ?? "Nieznany nauczyciel";

            var room = new RoomRepository().GetRoomById(lesson.RoomID);
            RoomName = room?.Number ?? "Nieznana sala";

            GridColumn = lesson.DayOfWeek switch
            {
                "Poniedziałek" => 0,
                "Wtorek" => 1,
                "Środa" => 2,
                "Czwartek" => 3,
                "Piątek" => 4,
                _ => 0
            };

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
            };

        }

    }
}
