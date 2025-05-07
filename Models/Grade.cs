using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_journal.Models;


    public class Grade
    {
        public int Id { get; set; }
        public int Value { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;
    }