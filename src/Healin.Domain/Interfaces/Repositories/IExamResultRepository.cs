using Healin.Domain.Models;
using Healin.Shared.Intefaces;
using System.Threading.Tasks;

namespace Healin.Domain.Interfaces.Repositories
{
    public interface IExamResultRepository : IRepository<ExamResult>
    {
        Task RemoveAsync(ExamResult examResult);
    }
}
