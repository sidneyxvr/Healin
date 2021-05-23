using Healin.Domain.Models;
using Healin.Domain.ValueObjects;
using Healin.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Domain.Interfaces.Repositories.ReadOnly
{
    public interface IDoctorReadOnlyRepository
    {
        Task<IList<Doctor>> GetAsync();
        Task<Address> GetAddressAsync(Guid doctorId);
        Task<PagedListDTO<Doctor>> GetPagedAsync(int page = 1, 
            int pageSize = 10, string search = "", string filter = "", string order = "");
        Task<PagedListDTO<Doctor>> GetPagedByPatientIdAsync(Guid patientId, int page = 1,
            int pageSize = 10, string search = "", string filter = "", string order = "");
    }
}
