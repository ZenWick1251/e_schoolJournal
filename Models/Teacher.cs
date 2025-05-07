using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_journal.Models;


    public class Teacher
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public List<Subject> Subjects { get; set; } = new();
    
    }