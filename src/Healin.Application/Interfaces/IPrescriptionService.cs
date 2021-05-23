using Healin.Application.Requests;
using Healin.Application.Responses;
using Healin.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Application.Interfaces
{
    public interface IPrescriptionService
    {
        Task AddAsync(PrescriptionRequest prescriptionRequest, IFormFile file);
        Task UpdateAsync(PrescriptionRequest prescriptionRequest);
        Task DeleteAsync(Guid id);

        Task<PagedListDTO<PrescriptionResponse>> GetPagedAsync(int page = 1,
            int pageSize = 10, string search = "", string filter = "", string order = "");
        Task<PrescriptionResponse> GetByIdAsync(Guid id);
        Task<IEnumerable<SelectItem<string>>> GetPrescriptionTypesAsync();
    }
}
