using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_journal.DTOs;
using e_journal.Models;

namespace e_journal.Interfaces
{
    public interface IGradeService
    {
        Task<GradeDto> AssignGradeAsync(GradeDto dto, User requester);
        Task<IEnumerable<GradeDto>> GetGradesByStudentIdAsync(int studentId, User requester);
        Task<IEnumerable<GradeDto>> GetAllGradesAsync(User requester);
        Task<bool> DeleteGradeAsync(int gradeId, User requester);
    }
}