using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_journal.DTOs;
using e_journal.Models;

namespace e_journal.Interfaces;

    public interface ISchoolClassService
    {
        Task<SchoolClassDto> CreateClassAsync(SchoolClassDto dto, User requester);
        Task<IEnumerable<SchoolClassDto>> GetAllClassesAsync(User requester);
        Task<SchoolClassDto?> GetClassByIdAsync(int id, User requester);
        Task<bool> DeleteClassAsync(int id, User requester);
    }
