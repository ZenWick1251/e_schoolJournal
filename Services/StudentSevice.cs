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

    public class StudentSevice : IStudentService
    {
        private readonly JournalDbContext _context;
        private readonly IMapper _mapper;

        public StudentSevice(JournalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<StudentDto> CreateStudentAsync(StudentDto dto, User requester)
        {
            if (requester.Role != "Admin")
                throw new UnauthorizedAccessException("Only admins can create students.");

            var student = _mapper.Map<Student>(dto);

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return _mapper.Map<StudentDto>(student);
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync(User requester)
        {
            if (requester.Role != "Admin" && requester.Role != "Teacher")
            throw new UnauthorizedAccessException("Only admins and teachers can view students.");

            var students = await _context.Students
                .Include(s => s.SchoolClass)
                .ToListAsync();

            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<StudentDto?> GetStudentByIdAsync(int id, User requester)
        {
            var student = await _context.Students
                .Include(s => s.SchoolClass)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
                return null;

            if (requester.Role == "Admin" || requester.Role == "Teacher" || requester.Id == student.UserId)
                return _mapper.Map<StudentDto>(student);

            throw new UnauthorizedAccessException("You do not have permission to view this student's data.");
        }

        public async Task<bool> DeleteStudentAsync(int id, User requester)
        {
            if (requester.Role != "Admin")
                throw new UnauthorizedAccessException("Only admins can delete students.");

            var student = await _context.Students.FindAsync(id);
            if (student == null) return false;

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return true;
        }
    }
