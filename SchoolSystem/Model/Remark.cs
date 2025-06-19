using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Model
{
    public class Remark
    {
        public int Id { get; set; }
        public int StudentID { get; set; }
        public int TeacherID { get; set; }
        public string? Value { get; set; }
        public string TeacherFullName { get; set; }

        public Remark(int id, int studentID, int teacherID, string? value, string teacherFullName)
        {
            Id = id;
            StudentID = studentID;
            TeacherID = teacherID;
            Value = value;
            TeacherFullName = teacherFullName;
        }

        public string? TeacherName { get; set; }
    }


}