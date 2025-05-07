using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_journal.DTOs;
using e_journal.Models;

namespace e_journal.Interfaces;

    public interface IUserService 
    {
        
        Task<UserDto> CreateUserAsync(CreateUserDto dto, User requester);

        Task<IEnumerable<UserDto>> GetAllUsersAsync(User requester);

        Task<bool> ChangeUserRoleAsync(int userId, string newRole, User requester);

        Task<User?> GetByUsernameAsync(string username);

        Task<User?> GetByIdAsync(int id);

    }
