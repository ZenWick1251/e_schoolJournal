using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_journal.Models;

        public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }

        public int SchoolClassId { get; set; }
        public SchoolClass SchoolClass { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public List<Grade> Grades { get; set; } = new();
    
    }


