using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Model
{
    public class Grade : Enums
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public GradeType Category { get; set; }
        public int Weight { get; set; }

        public Grade(int iD, int studentID, int subjectID, DateTime date, decimal value, GradeType category, int weight)
        {
            ID = iD;
            StudentID = studentID;
            SubjectID = subjectID;
            Date = date;
            Value = value;
            Category = category;
            Weight = weight;
        }
    }
}
