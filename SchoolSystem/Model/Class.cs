using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Model
{
    public class Class
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int ClassTeacherID { get; set; }
        public string TeacherFullName { get; set; }

        public Class(string code, int classTeacherID, int id, string teacherFullName)
        {
            Code = code;
            ClassTeacherID = classTeacherID;
            Id = id;
            TeacherFullName = teacherFullName;
        }
        public Class() { }
    }

}