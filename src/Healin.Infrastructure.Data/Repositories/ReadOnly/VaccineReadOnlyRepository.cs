using Healin.Domain.Interfaces.Repositories.ReadOnly;
using Healin.Domain.Models;
using Healin.Infrastructure.Data.Context;
using Healin.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healin.Infrastructure.Data.Repositories.ReadOnly
{
    public class VaccineReadOnlyRepository : BaseReadOnlyRepository<Vaccine>, IVaccineReadOnlyRepository
    {
        public VaccineReadOnlyRepository(HealinDbContext context) : base(context)
        {
        }

        public async Task<IList<Vaccine>> GetAsync()
        {
            return await _dbSet.Where(vaccine => vaccine.IsActive).ToListAsync();
        }

        public async Task<PagedListDTO<Vaccine>> GetPagedAsync(int page = 1, 
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var consulta = _dbSet.AsNoTrackingWithIdentityResolution();
            consulta = FilterOut(consulta, filter);
            consulta = Search(consulta, search);
            var ordenado = Order(consulta, order);
            ordenado = Page(ordenado, page, pageSize);
            return new PagedListDTO<Vaccine>
            (
                await ordenado.ToListAsync(),
                await consulta.CountAsync()
            );
        }

        protected override IQueryable<Vaccine> FilterOut(IQueryable<Vaccine> query, string filter)
        {
            return query;
        }

        protected override IQueryable<Vaccine> Order(IQueryable<Vaccine> query, string order)
        {
            return query;
        }

        protected override IQueryable<Vaccine> Search(IQueryable<Vaccine> query, string search)
        {
            return query;
        }
    }
}
