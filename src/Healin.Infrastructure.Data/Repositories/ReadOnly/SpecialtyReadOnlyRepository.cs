using Healin.Domain.Interfaces.Repositories.ReadOnly;
using Healin.Domain.Models;
using Healin.Infrastructure.Data.Context;
using Healin.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healin.Infrastructure.Data.Repositories.ReadOnly
{
    public class SpecialtyReadOnlyRepository : BaseReadOnlyRepository<Specialty>, ISpecialtyReadOnlyRepository
    {
        public SpecialtyReadOnlyRepository(HealinDbContext context) : base(context)
        {
        }

        public async Task<IList<Specialty>> GetAsync()
        {
            return await _dbSet.Where(specialty => specialty.IsActive).ToListAsync();
        }

        public async Task<PagedListDTO<Specialty>> GetPagedAsync(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var consulta = _dbSet.AsNoTrackingWithIdentityResolution();
            consulta = FilterOut(consulta, filter);
            consulta = Search(consulta, search);
            var ordenado = Order(consulta, order);
            ordenado = Page(ordenado, page, pageSize);
            return new PagedListDTO<Specialty>
            (
                await ordenado.ToListAsync(),
                await consulta.CountAsync()
            );
        }

        protected override IQueryable<Specialty> FilterOut(IQueryable<Specialty> query, string filter)
        {
            return query;
        }

        protected override IQueryable<Specialty> Order(IQueryable<Specialty> query, string order)
        {
            return query.OrderByDescending(examType => examType.Created);
        }

        protected override IQueryable<Specialty> Search(IQueryable<Specialty> query, string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                return query.Where(examType => EF.Functions.Contains(examType.Name, search));
            }
            return query;
        }
    }
}
