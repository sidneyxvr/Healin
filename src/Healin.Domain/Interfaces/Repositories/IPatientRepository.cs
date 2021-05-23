using Healin.Domain.Models;
using Healin.Shared.Intefaces;
using System;
using System.Threading.Tasks;

namespace Healin.Domain.Interfaces.Repositories
{
    public interface IPatientRepository : IRepository<Patient>
    {
        Task<bool> EmailInUseAsync(Guid patientId, string email);
        Task<bool> EmailInUseAsync(string email);
        Task<bool> CpfInUseAsync(Guid patientId, string cpf);
        Task<bool> CpfInUseAsync(string cpf);
        Task<bool> SusNumberInUseAsync(string susNumber);
        Task<bool> SusNumberInUseAsync(Guid patientId, string susNumber);
    }
}
