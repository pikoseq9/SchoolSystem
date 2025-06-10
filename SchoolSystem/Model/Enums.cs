using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Model
{
    public static class Enums
    {
        //np. typy użytkownika, ocen itp

        public enum GradeType
        {
            Exam,
            Semester,
            Final
        }
        public enum UserType
        {
            Principal,
            Teacher,
            Student
        }
        public enum Sex
        {
            F,
            M,
            U
        }
    }
}
