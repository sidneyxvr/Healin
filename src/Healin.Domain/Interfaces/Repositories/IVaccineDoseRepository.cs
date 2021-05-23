using Healin.Domain.Models;
using Healin.Shared.Intefaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Domain.Interfaces.Repositories
{
    public interface IVaccineDoseRepository : IRepository<VaccineDose>
    {
        Task<IList<VaccineDose>> GetByPatientIdAndVaccineIdAsync(Guid patientId, Guid vaccineId);
        Task RemoveAsync(VaccineDose vaccineDose);
    }
}
