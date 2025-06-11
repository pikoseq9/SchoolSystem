using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SchoolSystem.Model.Enums;

namespace SchoolSystem.Model
{
    public class Teacher
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? SurName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public Teacher(int id, string? name, string? surName, DateTime? dateOfBirth, string? gender, string? phoneNumber, string? login, string? password)
        {
            Id = id;
            Name = name;
            SurName = surName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            PhoneNumber = phoneNumber;
            Login = login;
            Password = password;
        }
    }
}