using Healin.Application.Requests;
using Healin.Application.Responses;
using Healin.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Application.Interfaces
{
    public interface IVaccineService
    {
        Task AddAsync(VaccineRequest vaccineRequest);
        Task UpdateAsync(VaccineRequest vaccineRequest);

        Task<PagedListDTO<VaccineResponse>> GetPagedAsync(int page = 1, 
            int pageSize = 10, string search = "", string filter = "", string order = "");
        Task<IList<SelectItem<Guid>>> GetAsync();
        Task<VaccineResponse> GetByIdAsync(Guid id);
    }
}
