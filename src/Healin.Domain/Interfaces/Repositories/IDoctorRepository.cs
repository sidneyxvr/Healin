using Healin.Domain.Models;
using Healin.Shared.Intefaces;
using System;
using System.Threading.Tasks;

namespace Healin.Domain.Interfaces.Repositories
{
    public interface IDoctorRepository : IRepository<Doctor>
    {
        Task<bool> EmailInUseAsync(Guid doctorId, string email);
        Task<bool> EmailInUseAsync(string email);
        Task<bool> CpfInUseAsync(Guid doctorId, string cpf);
        Task<bool> CpfInUseAsync(string cpf);
        Task<bool> CrmInUseAsync(Guid doctorId, string crm);
        Task<bool> CrmInUseAsync(string crm);
    }
}
