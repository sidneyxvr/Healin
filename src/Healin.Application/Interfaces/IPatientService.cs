using Healin.Application.Requests;
using Healin.Application.Responses;
using Healin.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Healin.Application.Interfaces
{
    public interface IPatientService
    {
        Task AddAsync(PatientRequest patientRequest);
        Task UpdateAsync(PatientRequest patientRequest);
        Task UpdateImageAsync(IFormFile image);
        Task UpdateAddressAsync(AddressRequest addressRequest);
        Task DeleteAsync(PatientRequest patientRequest);
        Task AddDoctorToMyDoctorsAsync(Guid doctorId);
        Task RemoveDoctorFromMyDoctorsAsync(Guid doctorId);
        Task DisableAsync(Guid id);

        Task<PagedListDTO<PatientResponse>> GetPagedByDoctorIdAsync(int page = 1,
            int pageSize = 10, string search = "", string filter = "", string order = "");
        Task<PatientResponse> GetByIdAsync(Guid id);
        Task<AddressResponse> GetAddressAsync();
    }
}
