using Healin.Domain.Interfaces.Repositories;
using Healin.Domain.Models;
using Healin.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healin.Infrastructure.Data.Repositories
{
    public class ExamTypeRepository : BaseRespository<ExamType>, IExamTypeRepository
    {
        public ExamTypeRepository(HealinDbContext context) : base(context)
        {
        }

        public override async Task<ExamType> GetByIdAsync(Guid id)
        {
            return await _dbSet
                .Include(examType => examType.Exam)
                .FirstOrDefaultAsync(examType => examType.Id == id);
        }

        public async Task<IList<ExamType>> GetByIdsAsync(Guid[] ids)
        {
            return await _dbSet
                .Where(examType => ids.Any(id => id == examType.Id))
                .ToListAsync();
        }
    }
}
