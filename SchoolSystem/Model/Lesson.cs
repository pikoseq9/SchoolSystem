using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SchoolSystem.Model.Enums;

namespace SchoolSystem.Model
{
    public class Lesson
    {
        public int Id { get; set; }
        public int RoomID { get; set; }
        public int SubjectID { get; set; }
        public int ClassID { get; set; }
        public int TeacherID { get; set; }
        public DateTime Date { get; set; }

        public Lesson(int id, int roomID, int subjectID, int classID, int teacherID, DateTime date)
        {
            Id = id;
            RoomID = roomID;
            SubjectID = subjectID;
            ClassID = classID;
            TeacherID = teacherID;
            Date = date;
        }
    }
}
