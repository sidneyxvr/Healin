using Healin.Domain.Interfaces.Repositories;
using Healin.Domain.Models;
using Healin.Infrastructure.Data.Context;

namespace Healin.Infrastructure.Data.Repositories
{
    public class ExamRepository : BaseRespository<Exam>, IExamRepository
    {
        public ExamRepository(HealinDbContext context) : base(context)
        {
        }
    }
}
