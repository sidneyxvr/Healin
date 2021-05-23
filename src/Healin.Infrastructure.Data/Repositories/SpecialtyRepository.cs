using Healin.Domain.Interfaces.Repositories;
using Healin.Domain.Models;
using Healin.Infrastructure.Data.Context;

namespace Healin.Infrastructure.Data.Repositories
{
    public class SpecialtyRepository : BaseRespository<Specialty>, ISpecialtyRepository
    {
        public SpecialtyRepository(HealinDbContext context) : base(context)
        {
        }
    }
}
