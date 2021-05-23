using Healin.Domain.Models;
using Healin.Shared.Intefaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Domain.Interfaces.Repositories
{
    public interface IExamTypeRepository : IRepository<ExamType>
    {
        Task<IList<ExamType>> GetByIdsAsync(Guid[] ids);
    }
}
