using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_journal.DTOs;


    public class LessonScheduleDto
    {
        public int Id { get; set; }
        public int SchoolClassId { get; set; }
        public int SubjectId { get; set; }
        public DateTime Date { get; set; }
    }