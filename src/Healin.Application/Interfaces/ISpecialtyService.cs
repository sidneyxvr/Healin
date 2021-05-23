using Healin.Application.Requests;
using Healin.Application.Responses;
using Healin.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Application.Interfaces
{
    public interface ISpecialtyService
    {
        Task AddAsync(SpecialtyRequest specialtyRequest);
        Task UpdateAsync(SpecialtyRequest specialtyRequest);
        Task DeleteAsync(SpecialtyRequest specialtyRequest);

        Task<PagedListDTO<SpecialtyResponse>> GetPagedAsync(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "");
        Task<IList<SelectItem<Guid>>> GetAsync();
        Task<SpecialtyResponse> GetByIdAsync(Guid id);
    }
}
