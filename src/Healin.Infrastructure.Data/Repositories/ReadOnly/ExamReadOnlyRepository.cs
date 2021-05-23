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
    public class ExamReadOnlyRepository : BaseReadOnlyRepository<Exam>, IExamReadOnlyRepository
    {
        public ExamReadOnlyRepository(HealinDbContext context) : base(context)
        {
        }

        public async Task<IList<Exam>> GetAsync()
        {
            return await _dbSet.AsNoTrackingWithIdentityResolution().ToListAsync();
        }

        public async Task<PagedListDTO<Exam>> GetPagedAsync(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var consulta = _dbSet.AsNoTrackingWithIdentityResolution();
            consulta = FilterOut(consulta, filter);
            consulta = Search(consulta, search);
            var ordenado = Order(consulta, order);
            ordenado = Page(ordenado, page, pageSize);
            return new PagedListDTO<Exam>
            (
                await ordenado.ToListAsync(),
                await consulta.CountAsync()
            );
        }

        protected override IQueryable<Exam> FilterOut(IQueryable<Exam> query, string filter)
        {
            return query;
        }

        protected override IQueryable<Exam> Order(IQueryable<Exam> query, string order)
        {
            return query.OrderBy(examType => examType.Name);
        }

        protected override IQueryable<Exam> Search(IQueryable<Exam> query, string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                return query.Where(examType => EF.Functions.Contains(examType.Name, search));
            }
            return query;
        }
    }
}
