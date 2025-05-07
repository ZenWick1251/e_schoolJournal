using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_journal.DTOs;
using e_journal.Models;

namespace e_journal.Interfaces;

    public interface ITeacherService
    {
        Task<TeacherDto> CreateTeacherAsync(TeacherDto dto, User requester);
        Task<IEnumerable<TeacherDto>> GetAllTeachersAsync(User requester);
        Task<TeacherDto?> GetTeacherByIdAsync(int id, User requester);
        Task<bool> DeleteTeacherAsync(int id, User requester);
    }
