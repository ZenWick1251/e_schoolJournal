using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_journal.Models;


    public class LessonSchedule
    {
        public int Id { get; set; }
        public int SchoolClassId { get; set; }
        public SchoolClass SchoolClass { get; set; } = null!;

        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;
    
        public DateTime Date { get; set; }
    }
