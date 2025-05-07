using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using e_journal.Data;
using e_journal.DTOs;
using e_journal.Interfaces;
using e_journal.Models;
using Microsoft.EntityFrameworkCore;

namespace e_journal.Services;

    public class LessonScheduleService : ILessonScheduleService
    {
        private readonly JournalDbContext _context;
        private readonly IMapper _mapper;

        public LessonScheduleService(JournalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<LessonScheduleDto> CreateLessonAsync(LessonScheduleDto dto, User requester)
        {
            if (requester.Role != "Admin")
                throw new UnauthorizedAccessException("Only admins can create lessons.");

            var schoolClass = await _context.SchoolClasses.FindAsync(dto.SchoolClassId);
            var subject = await _context.Subjects.FindAsync(dto.SubjectId);

            if (schoolClass == null || subject == null)
                throw new ArgumentException("Invalid class or subject.");

            var lesson = _mapper.Map<LessonSchedule>(dto);

            _context.LessonSchedules.Add(lesson);
            await _context.SaveChangesAsync();

            return _mapper.Map<LessonScheduleDto>(lesson);
        }

        public async Task<IEnumerable<LessonScheduleDto>> GetScheduleByClassIdAsync(int classId, User requester)
        {
            if (requester.Role == "Student")
            {
                var student = await _context.Students.FirstOrDefaultAsync(s => s.UserId == requester.Id);
                if (student == null || student.SchoolClassId != classId)
                    throw new UnauthorizedAccessException("Access denied to this schedule.");
            }

            var lessons = await _context.LessonSchedules
                .Where(l => l.SchoolClassId == classId)
                .Include(l => l.Subject)
                .ToListAsync();

            return _mapper.Map<IEnumerable<LessonScheduleDto>>(lessons);
        }

        public async Task<IEnumerable<LessonScheduleDto>> GetAllLessonsAsync(User requester)
        {
            if (requester.Role != "Admin" && requester.Role != "Teacher")
                throw new UnauthorizedAccessException("Only admins and teachers can view all lessons.");

            var lessons = await _context.LessonSchedules
                .Include(l => l.Subject)
                .Include(l => l.SchoolClass)
                .ToListAsync();

            return _mapper.Map<IEnumerable<LessonScheduleDto>>(lessons);
        }

        public async Task<bool> DeleteLessonAsync(int lessonId, User requester)
        {
            if (requester.Role != "Admin")
                throw new UnauthorizedAccessException("Only admins can delete lessons.");

            var lesson = await _context.LessonSchedules.FindAsync(lessonId);
            if (lesson == null) return false;

            _context.LessonSchedules.Remove(lesson);
            await _context.SaveChangesAsync();

            return true;
        }
    }
