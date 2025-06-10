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
        public string Code { get; }
        public int ClassTeacherID { get; set; }
        public Class(string code, int classTeacherID, int id)
        {
            Code = code;
            ClassTeacherID = classTeacherID;
            Id = id;
        }
        
    }
}
