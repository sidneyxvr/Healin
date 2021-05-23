using Healin.Application.Requests;
using Healin.Application.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Application.Interfaces
{
    public interface IVaccineDoseService
    {
        Task AddAsync(VaccineDoseRequest vaccineDoseRequest);
        Task RemoveAsync(Guid id);

        Task<IEnumerable<SelectItem<string>>> GetDoseTypesAsync();
        Task<IEnumerable<(string VaccineName, List<VaccineDoseResponse> VaccineDoses)>> GetByPatientIdAsync(Guid patientId);
    }
}
