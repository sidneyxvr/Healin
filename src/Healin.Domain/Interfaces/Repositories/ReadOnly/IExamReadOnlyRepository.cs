using Healin.Domain.Models;
using Healin.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Domain.Interfaces.Repositories.ReadOnly
{
    public interface IExamReadOnlyRepository
    {
        Task<IList<Exam>> GetAsync();
        Task<PagedListDTO<Exam>> GetPagedAsync(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "");
    }
}
