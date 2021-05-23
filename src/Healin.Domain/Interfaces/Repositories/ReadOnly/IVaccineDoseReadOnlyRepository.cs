using Healin.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Domain.Interfaces.Repositories.ReadOnly
{
    public interface IVaccineDoseReadOnlyRepository
    {
        Task<IList<VaccineDose>> GetAsync();
        Task<IList<VaccineDose>> GetByPatientIdAsync(Guid patientId);
    }
}
