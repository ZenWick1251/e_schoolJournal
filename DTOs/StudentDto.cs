using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_journal.DTOs;


    public class StudentDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime DateOfBirth { get; set; }
        public int SchoolClassId { get; set; }

        public string ClassName { get; set; } = null!;
    }