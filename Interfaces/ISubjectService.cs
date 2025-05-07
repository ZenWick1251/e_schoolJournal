using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_journal.DTOs;
using e_journal.Models;

namespace e_journal.Interfaces;

    public interface ISubjectService
    {
        Task<SubjectDto> CreateSubjectAsync(SubjectDto dto, User requester);
        Task<IEnumerable<SubjectDto>> GetAllSubjectsAsync(User requester);
        Task<SubjectDto?> GetSubjectByIdAsync(int id, User requester);
        Task<bool> DeleteSubjectAsync(int id, User requester);
    }
