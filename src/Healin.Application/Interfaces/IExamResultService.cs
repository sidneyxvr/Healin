using Healin.Application.Requests;
using Healin.Application.Responses;
using Healin.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Healin.Application.Interfaces
{
    public interface IExamResultService
    {
        Task AddAsync(ExamResultRequest examResultRequest, IFormFile file);
        Task UpdateAsync(ExamResultRequest examResultRequest);
        Task DeleteAsync(Guid id);

        Task<ExamResultResponse> GetByIdAsync(Guid id);
        Task<PagedListDTO<ExamResultResponse>> GetPagedAsync(int page = 1, 
            int pageSize = 10, string search = "", string filter = "", string order = "");
        Task<PagedListDTO<ExamResultResponse>> GetPagedByPatientIdAsync(Guid patientId, 
            int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "");
    }
}
