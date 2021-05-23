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
    public class ExamResultReadOnlyRepository : BaseReadOnlyRepository<ExamResult>, IExamResultReadOnlyRepository
    {
        public ExamResultReadOnlyRepository(HealinDbContext context) : base(context)
        {
        }

        public Task<IList<ExamResult>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedListDTO<ExamResult>> GetPagedAsync(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var consulta = _dbSet.AsNoTrackingWithIdentityResolution();
            consulta = FilterOut(consulta, filter);
            consulta = Search(consulta, search);
            var ordenado = Order(consulta, order);
            ordenado = Page(ordenado, page, pageSize);
            return new PagedListDTO<ExamResult>
            (
                await ordenado
                    .Include(examResult => examResult.Exam)
                    .Include(examResult => examResult.ExamTypes)
                    .ToListAsync(),
                await consulta.CountAsync()
            );
        }

        public async Task<PagedListDTO<ExamResult>> GetPagedByPatientIdAsync(Guid patientId, 
            int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var query = _dbSet.AsNoTrackingWithIdentityResolution().Where(e => e.PatientId == patientId);
            query = FilterOut(query, filter);
            query = Search(query, search);
            var ordered = Order(query, order);
            ordered = Page(ordered, page, pageSize);
            return new PagedListDTO<ExamResult>
            (
                await ordered   
                    .Include(examResult => examResult.Exam)
                    .Include(examResult => examResult.ExamTypes)
                    .ToListAsync(),
                await query.CountAsync()
            );
        }

        protected override IQueryable<ExamResult> FilterOut(IQueryable<ExamResult> query, string filter)
        {
            return query;
        }

        protected override IQueryable<ExamResult> Order(IQueryable<ExamResult> query, string order)
        {
            var _order = order?.Trim();

            return _order switch
            {
                "description" => query.OrderBy(examResult => examResult.Description),
                "examDate" => query.OrderBy(examType => examType.ExamDate),
                "exam" => query.OrderBy(examType => examType.Exam.Name),
                _ => query.OrderBy(examType => examType.Created),
            };
        }

        protected override IQueryable<ExamResult> Search(IQueryable<ExamResult> query, string search)
        {
            return string.IsNullOrWhiteSpace(search) ?
                query :
                query.Where(examResult => examResult.Description.Contains(search) || 
                    examResult.Exam.Name.Contains(search) || 
                    examResult.ExamTypes.Any(examType => examType.Name.Contains(search)));
        }
    }
}
