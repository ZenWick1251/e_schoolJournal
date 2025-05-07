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

    public class GradeService : IGradeService
    {
        private readonly JournalDbContext _context;
        private readonly IMapper _mapper;

        public GradeService(JournalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GradeDto> AssignGradeAsync(GradeDto dto, User requester)
        {
            if (requester.Role != "Teacher")
                throw new UnauthorizedAccessException("Only teachers can assign grades.");

            var student = await _context.Students.FindAsync(dto.StudentId);
            var subject = await _context.Subjects.FindAsync(dto.SubjectId);

            if (student == null || subject == null) 
                throw new ArgumentException("Invalid student or subject.");

       
            if (subject.TeacherId != requester.Teacher?.Id)
                throw new UnauthorizedAccessException("You can only assign grades to your own subjects.");

            var grade = _mapper.Map<Grade>(dto);

            _context.Grades.Add(grade);
            await _context.SaveChangesAsync();

            return _mapper.Map<GradeDto>(grade);
        }

        public async Task<IEnumerable<GradeDto>> GetGradesByStudentIdAsync(int studentId, User requester)
        {
            var student = await _context.Students
                .Include(s => s.Grades)
                .ThenInclude(g => g.Subject)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
                throw new ArgumentException("Student not found.");

            if (requester.Role == "Admin" ||
                (requester.Role == "Student" && requester.Id == student.UserId) ||
                (requester.Role == "Teacher"))
            {
                return _mapper.Map<IEnumerable<GradeDto>>(student.Grades);
            }

            throw new UnauthorizedAccessException("You cannot view these grades.");
        }

        public async Task<IEnumerable<GradeDto>> GetAllGradesAsync(User requester)
        {
            if (requester.Role != "Admin" && requester.Role != "Teacher")
                throw new UnauthorizedAccessException("Only admins and teachers can view all grades.");

            var grades = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject)
                .ToListAsync();

            return _mapper.Map<IEnumerable<GradeDto>>(grades);
        }

        public async Task<bool> DeleteGradeAsync(int gradeId, User requester)
        {
            if (requester.Role != "Admin")
                throw new UnauthorizedAccessException("Only admins can delete grades.");

            var grade = await _context.Grades.FindAsync(gradeId);
            if (grade == null) return false;

            _context.Grades.Remove(grade);
            await _context.SaveChangesAsync();

            return true;
        }
    }
