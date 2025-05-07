using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_journal.DTOs;
using e_journal.Models;

namespace e_journal.Interfaces;

    public interface IStudentService
    {
        
        Task<StudentDto> CreateStudentAsync(StudentDto dto, User requester);

        Task<IEnumerable<StudentDto>> GetAllStudentsAsync(User requester);

        Task<StudentDto?> GetStudentByIdAsync(int id, User requester);

        Task<bool> DeleteStudentAsync(int id, User requester);
    }
