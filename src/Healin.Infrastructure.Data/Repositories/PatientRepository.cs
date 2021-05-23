using Healin.Domain.Interfaces.Repositories;
using Healin.Domain.Models;
using Healin.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Healin.Infrastructure.Data.Repositories
{
    public class PatientRepository : BaseRespository<Patient>, IPatientRepository
    {
        public PatientRepository(HealinDbContext context) : base(context)
        {
        }

        public async Task<bool> CpfInUseAsync(Guid patientId, string cpf)
        {
            return await _dbSet.AsNoTracking().AnyAsync(p => p.Id != patientId && p.Cpf.Value == cpf);
        }

        public async Task<bool> CpfInUseAsync(string cpf)
        {
            return await _dbSet.AsNoTracking().AnyAsync(p => p.Cpf.Value == cpf);
        }

        public async Task<bool> EmailInUseAsync(Guid patientId, string email)
        {
            return await _dbSet.AsNoTracking().AnyAsync(p => p.Id != patientId && p.Email.Value == email);
        }

        public async Task<bool> EmailInUseAsync(string email)
        {
            return await _dbSet.AsNoTracking().AnyAsync(p => p.Email.Value == email);
        }

        public override async Task<Patient> GetByIdAsync(Guid id, params string[] joins)
        {
            return await _dbSet.Includes(joins).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> SusNumberInUseAsync(Guid patientId, string susNumber)
        {
            return await _dbSet.AsNoTracking().AnyAsync(p => p.Id != patientId && p.SusNumber == susNumber);
        }

        public async Task<bool> SusNumberInUseAsync(string susNumber)
        {
            return await _dbSet.AsNoTracking().AnyAsync(p => p.SusNumber == susNumber);
        }
    }

    public static class PatientExtensions
    {
        public static IQueryable<Patient> Includes(this IQueryable<Patient> query, params string[] joins)
        {
            return joins.Aggregate(query, (current, @join) => @join switch
            {
                nameof(Patient.Doctors) => current.Include(p => p.Doctors),
                nameof(Doctor.Address) => current.Include(d => d.Address),
                _ => current
            });
        }
    }
}
