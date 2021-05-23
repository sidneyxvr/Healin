using Healin.Application.Requests;
using Healin.Application.Responses;
using Healin.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Application.Interfaces
{
    public interface IExamService
    {
        Task AddAsync(ExamRequest examRequest);
        Task UpdateAsync(ExamRequest examRequest);
        Task DeleteAsync(ExamRequest examRequest);

        Task<PagedListDTO<ExamResponse>> GetPagedAsync(int page = 1, 
            int pageSize = 10, string search = "", string filter = "", string order = "");
        Task<ExamResponse> GetByIdAsync(Guid id);
        Task<List<SelectItem<Guid>>> GetAsync();
    }
}
