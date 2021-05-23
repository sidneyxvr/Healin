using Healin.Domain.Interfaces.Repositories;
using Healin.Domain.Models;
using Healin.Infrastructure.Data.Context;

namespace Healin.Infrastructure.Data.Repositories
{
    public class VaccineRepository : BaseRespository<Vaccine>, IVaccineRepository
    {
        public VaccineRepository(HealinDbContext context) : base(context)
        {
        }
    }
}
