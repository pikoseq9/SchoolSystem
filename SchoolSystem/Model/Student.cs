using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Model
{
    public class Student : Enums
    {
        public int Id { get; set; }
        public int ClassID { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public Sex? Gender { get; set; }
        public string? PESEL { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public Student(int id, int classID, string? name, string? surName, DateTime? dateOfBirth, Sex? gender, string? pESEL, string? login, string? password)
        {
            Id = id;
            ClassID = classID;
            Name = name;
            SurName = surName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            PESEL = pESEL;
            Login = login;
            Password = password;
        }
    }
}
