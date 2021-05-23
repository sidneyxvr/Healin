using Healin.Application.Requests;
using Healin.Application.Responses;
using Healin.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Application.Interfaces
{
    public interface IExamTypeService
    {
        Task AddAsync(ExamTypeRequest examTypeRequest);
        Task UpdateAsync(ExamTypeRequest examTypeRequest);
        Task DeleteAsync(ExamTypeRequest examTypeRequest);

        Task<PagedListDTO<ExamTypeResponse>> GetPagedAsync(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "");
        Task<ExamTypeResponse> GetByIdAsync(Guid id);
        Task<IEnumerable<SelectItem<Guid>>> GetByExamIdAsync(Guid examId);
    }
}
