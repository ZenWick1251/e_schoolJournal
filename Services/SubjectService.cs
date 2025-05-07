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

    public class SubjectService : ISubjectService
    {
        private readonly JournalDbContext _context;
        private readonly IMapper _mapper;

        public SubjectService(JournalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SubjectDto> CreateSubjectAsync(SubjectDto dto, User requester)
        {
            if (requester.Role != "Admin")
                throw new UnauthorizedAccessException("Only admins can create subjects.");

            var teacher = await _context.Teachers.FindAsync(dto.TeacherId);
            if (teacher == null)
                throw new ArgumentException("Teacher not found.");

            var subject = _mapper.Map<Subject>(dto);

            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();

            return _mapper.Map<SubjectDto>(subject);
        }

        public async Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync(User requester)
        {
            if (requester.Role != "Admin" && requester.Role != "Teacher")
                throw new UnauthorizedAccessException("Only admins and teachers can view subjects.");

            var subjects = await _context.Subjects
                .Include(s => s.Teacher)
                .ToListAsync();

            return _mapper.Map<IEnumerable<SubjectDto>>(subjects);
        }

        public async Task<SubjectDto?> GetSubjectByIdAsync(int id, User requester)
        {
            var subject = await _context.Subjects
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (subject == null)
                return null;

            if (requester.Role == "Admin" ||
                (requester.Role == "Teacher" && requester.Id == subject.Teacher.UserId))
            {
            return _mapper.Map<SubjectDto>(subject);
            }

            throw new UnauthorizedAccessException("Access denied.");
        }

        public async Task<bool> DeleteSubjectAsync(int id, User requester)
        {
            if (requester.Role != "Admin")
                throw new UnauthorizedAccessException("Only admins can delete subjects.");

            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null) return false;

            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return true;
        }
    }
