using Healin.Domain.Models;
using Healin.Domain.ValueObjects;
using Healin.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Domain.Interfaces.Repositories.ReadOnly
{
    public interface IPatientReadOnlyRepository
    {
        Task<IList<Patient>> GetAsync();
        Task<Address> GetAddressAsync(Guid patientId);
        Task<PagedListDTO<Patient>> GetPagedAsync(int page = 1,
            int pageSize = 10, string search = "", string filter = "", string order = "");
        Task<PagedListDTO<Patient>> GetPagedByDoctorIdAsync(Guid doctorId, int page = 1,
            int pageSize = 10, string search = "", string filter = "", string order = "");
    }
}
