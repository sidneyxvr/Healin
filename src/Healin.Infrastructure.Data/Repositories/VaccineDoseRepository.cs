using Healin.Domain.Interfaces.Repositories;
using Healin.Domain.Models;
using Healin.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healin.Infrastructure.Data.Repositories
{
    public class VaccineDoseRepository : BaseRespository<VaccineDose>, IVaccineDoseRepository
    {
        public VaccineDoseRepository(HealinDbContext context) : base(context)
        {
        }
        
        public async Task<IList<VaccineDose>> GetByPatientIdAndVaccineIdAsync(Guid patientId, Guid vaccineId)
        {
            return await _dbSet.Where(vaccineDose => vaccineDose.PatientId == patientId && vaccineDose.VaccineId == vaccineId).ToListAsync();
        }

        public Task RemoveAsync(VaccineDose vaccineDose)
        {
            _dbSet.Remove(vaccineDose);
            return Task.CompletedTask;
        }
    }
}
