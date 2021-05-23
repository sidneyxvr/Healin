using Healin.Domain.Models;
using Healin.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Domain.Interfaces.Repositories.ReadOnly
{
    public interface IVaccineReadOnlyRepository
    {
        Task<IList<Vaccine>> GetAsync();
        Task<PagedListDTO<Vaccine>> GetPagedAsync(int page = 1, 
            int pageSize = 10, string search = "", string filter = "", string order = "");
    }
}
