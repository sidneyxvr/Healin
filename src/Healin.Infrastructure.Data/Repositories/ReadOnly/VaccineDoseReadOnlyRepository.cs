using Healin.Domain.Interfaces.Repositories.ReadOnly;
using Healin.Domain.Models;
using Healin.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healin.Infrastructure.Data.Repositories.ReadOnly
{
    public class VaccineDoseReadOnlyRepository : BaseReadOnlyRepository<VaccineDose>, IVaccineDoseReadOnlyRepository
    {
        public VaccineDoseReadOnlyRepository(HealinDbContext context) : base(context)
        {
        }

        public Task<IList<VaccineDose>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IList<VaccineDose>> GetByPatientIdAsync(Guid patientId)
        {
            return await _dbSet.AsNoTrackingWithIdentityResolution()
                .Where(vaccineDose => vaccineDose.PatientId == patientId)
                .Include(vaccineDose => vaccineDose.Vaccine)
                .ToListAsync();
        }
    }
}
