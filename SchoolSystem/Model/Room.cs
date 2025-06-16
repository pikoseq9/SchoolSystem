using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolSystem.Model
{
    public class Room
    {
        public int ID { get; set; }
        public string? Number { get; set; }
        public Room(int id, string? number)
        {
            ID = id;
            Number = number;
        }
    }
}