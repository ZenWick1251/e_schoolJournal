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

    public class UserService : IUserService
    {
        private readonly JournalDbContext _context;
        private readonly IMapper _mapper;

        public UserService(JournalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserDto dto, User requester)
        {
            if (requester.Role != "Admin")
                throw new UnauthorizedAccessException("Only admins can create users.");

            if (await _context.Users.AnyAsync(u => u.Username == dto.Username))
                throw new InvalidOperationException("Username already exists.");

            var user = new User
            {
                Username = dto.Username,
                Password = dto.Password,
                Role = dto.Role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(User requester)
        {
            if (requester.Role != "Admin")
                throw new UnauthorizedAccessException("Only admins can view all users.");

            var users = await _context.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<bool> ChangeUserRoleAsync(int userId, string newRole, User requester)
        {
            if (requester.Role != "Admin")
                throw new UnauthorizedAccessException("Only admins can change roles.");

            if (requester.Id == userId)
                throw new InvalidOperationException("Admin cannot change their own role.");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            user.Role = newRole;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Student)
                .Include(u => u.Teacher)
                .FirstOrDefaultAsync(u => u.Id == id);
        }


        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
