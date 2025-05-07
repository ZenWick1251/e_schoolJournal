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

    public class SchoolCLassService : ISchoolClassService
    {
        private readonly JournalDbContext _context;
        private readonly IMapper _mapper;

        public SchoolCLassService(JournalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SchoolClassDto> CreateClassAsync(SchoolClassDto dto, User requester)
        {
            if (requester.Role != "Admin")
                throw new UnauthorizedAccessException("Only admins can create school classes.");

            var schoolClass = _mapper.Map<SchoolClass>(dto);

            _context.SchoolClasses.Add(schoolClass);
            await _context.SaveChangesAsync();

            return _mapper.Map<SchoolClassDto>(schoolClass);
        }

        public async Task<IEnumerable<SchoolClassDto>> GetAllClassesAsync(User requester)
        {
            if (requester.Role != "Admin" && requester.Role != "Teacher")
                throw new UnauthorizedAccessException("Only admins and teachers can view classes.");

            var classes = await _context.SchoolClasses.ToListAsync();
            return _mapper.Map<IEnumerable<SchoolClassDto>>(classes);
        }

        public async Task<SchoolClassDto?> GetClassByIdAsync(int id, User requester)
        {
            var schoolClass = await _context.SchoolClasses
                .Include(c => c.Students)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (schoolClass == null)
                return null;

            if (requester.Role == "Admin" || requester.Role == "Teacher")
                return _mapper.Map<SchoolClassDto>(schoolClass);

            throw new UnauthorizedAccessException("Access denied.");
        }

        public async Task<bool> DeleteClassAsync(int id, User requester)
        {
            if (requester.Role != "Admin")
                throw new UnauthorizedAccessException("Only admins can delete school classes.");

            var schoolClass = await _context.SchoolClasses.FindAsync(id);
            if (schoolClass == null) return false;

            _context.SchoolClasses.Remove(schoolClass);
            await _context.SaveChangesAsync();

            return true;
        }
    }
