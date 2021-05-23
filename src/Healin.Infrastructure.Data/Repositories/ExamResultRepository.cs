using Healin.Domain.Interfaces.Repositories;
using Healin.Domain.Models;
using Healin.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Healin.Infrastructure.Data.Repositories
{
    public class ExamResultRepository : BaseRespository<ExamResult>, IExamResultRepository
    {
        public ExamResultRepository(HealinDbContext context) : base(context)
        {
        }

        public override async Task<ExamResult> GetByIdAsync(Guid id, params string[] joins)
        {
            return await _dbSet.Includes(joins).FirstOrDefaultAsync(e => e.Id == id);
        }

        public Task RemoveAsync(ExamResult examResult)
        {
            _dbSet.Remove(examResult);
            return Task.CompletedTask;
        }
    }

    public static class ExamResultExtensions
    {
        public static IQueryable<ExamResult> Includes(this IQueryable<ExamResult> query, params string[] joins)
        {
            return joins.Aggregate(query, (current, @join) => @join switch
            {
                nameof(ExamResult.ExamTypes) => current.Include(e => e.ExamTypes),
                nameof(ExamResult.Exam) => current.Include(e => e.Exam),
                _ => current
            });
        }
    }
}
