using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_journal.DTOs;
using e_journal.Models;

namespace e_journal.Interfaces;

    public interface ILessonScheduleService
    {
        Task<LessonScheduleDto> CreateLessonAsync(LessonScheduleDto dto, User requester);
        Task<IEnumerable<LessonScheduleDto>> GetScheduleByClassIdAsync(int classId, User requester);
        Task<IEnumerable<LessonScheduleDto>> GetAllLessonsAsync(User requester);
        Task<bool> DeleteLessonAsync(int lessonId, User requester);
    }