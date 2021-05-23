using Healin.Domain.Interfaces.Repositories;
using Healin.Domain.Models;
using Healin.Infrastructure.Data.Context;
using System.Threading.Tasks;

namespace Healin.Infrastructure.Data.Repositories
{
    public class PrescriptionRepository : BaseRespository<Prescription>, IPrescriptionRepository
    {
        public PrescriptionRepository(HealinDbContext context) : base(context)
        {
        }

        public Task RemoveAsync(Prescription prescription)
        {
            _dbSet.Remove(prescription);
            return Task.CompletedTask;
        }
    }
}
