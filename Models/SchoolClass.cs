using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_journal.Models;


    public class SchoolClass
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    
        public List<Student> Students { get; set; } = new();
    }