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
    public class ExamTypeReadOnlyRepository : BaseReadOnlyRepository<ExamType>, IExamTypeReadOnlyRepository
    {
        public ExamTypeReadOnlyRepository(HealinDbContext context) : base(context)
        {
        }

        public async Task<IList<ExamType>> GetByExamIdAsync(Guid examId)
        {
            return await _dbSet.Where(e => e.ExamId == examId).ToListAsync();
        }

        public async Task<PagedListDTO<ExamType>> GetPagedAsync(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var consulta = _dbSet.AsNoTrackingWithIdentityResolution();
            consulta = FilterOut(consulta, filter);
            consulta = Search(consulta, search);
            var ordenado = Order(consulta, order);
            ordenado = Page(ordenado, page, pageSize);
            return new PagedListDTO<ExamType>
            (
                await ordenado.Include(examType => examType.Exam).ToListAsync(),
                await consulta.CountAsync()
            );
        }

        protected override IQueryable<ExamType> FilterOut(IQueryable<ExamType> query, string filter)
        {
            return query;
        }

        protected override IQueryable<ExamType> Order(IQueryable<ExamType> query, string order)
        {
            return query.OrderByDescending(examType => examType.Created);
        }

        protected override IQueryable<ExamType> Search(IQueryable<ExamType> query, string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                return query.Where(examType => EF.Functions.Contains(examType.Name, search));
            }
            return query;
        }
    }
}
