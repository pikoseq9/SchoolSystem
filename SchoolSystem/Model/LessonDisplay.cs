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
    }
}
