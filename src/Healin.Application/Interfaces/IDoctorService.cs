using Healin.Application.Requests;
using Healin.Application.Responses;
using Healin.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Healin.Application.Interfaces
{
    public interface IDoctorService
    {
        Task AddAsync(DoctorRequest doctorRequest);
        Task UpdateAsync(DoctorRequest doctorRequest);
        Task UpdateImageAsync(IFormFile image);
        Task UpdateAddressAsync(AddressRequest addressRequest);
        Task DeleteAsync(Guid id);
        Task DisableAsync(Guid id);

        Task<PagedListDTO<DoctorResponse>> GetPagedAsync(int page = 1, 
            int pageSize = 10, string search = "", string filter = "", string order = "");
        Task<PagedListDTO<DoctorResponse>> GetPagedByPatientAsync(int page = 1,
            int pageSize = 10, string search = "", string filter = "", string order = "");
        Task<DoctorResponse> GetByIdAsync(Guid id);
        Task<AddressResponse> GetAddressAsync();
    }
}
