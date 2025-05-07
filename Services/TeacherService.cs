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

    public class TeacherService : ITeacherService 
    {
        private readonly JournalDbContext _context;
        private readonly IMapper _mapper;

        public TeacherService(JournalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TeacherDto> CreateTeacherAsync(TeacherDto dto, User requester)
        {
            if (requester.Role != "Admin")
                throw new UnauthorizedAccessException("Only admins can create teachers.");

            var teacher = _mapper.Map<Teacher>(dto);

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            return _mapper.Map<TeacherDto>(teacher);
        }

        public async Task<IEnumerable<TeacherDto>> GetAllTeachersAsync(User requester)
        {
            if (requester.Role != "Admin")
                throw new UnauthorizedAccessException("Only admins can view all teachers.");

            var teachers = await _context.Teachers.ToListAsync();
            return _mapper.Map<IEnumerable<TeacherDto>>(teachers);
        }

        public async Task<TeacherDto?> GetTeacherByIdAsync(int id, User requester)
        {
            var teacher = await _context.Teachers
                .Include(t => t.Subjects)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (teacher == null)
                return null;

            if (requester.Role == "Admin" || requester.Id == teacher.UserId)
                return _mapper.Map<TeacherDto>(teacher);

            throw new UnauthorizedAccessException("Access denied.");
        }

        public async Task<bool> DeleteTeacherAsync(int id, User requester)
        {
            if (requester.Role != "Admin")
                throw new UnauthorizedAccessException("Only admins can delete teachers.");

            var teacher = await _context.Teachers.FindAsync(id);
            if (teacher == null) return false;

            _context.Teachers.Remove(teacher);
            await _context.SaveChangesAsync();

            return true;
        }
    }
