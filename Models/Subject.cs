using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_journal.Models;


    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;
    
        public List<Grade> Grades { get; set; } = new();
    }