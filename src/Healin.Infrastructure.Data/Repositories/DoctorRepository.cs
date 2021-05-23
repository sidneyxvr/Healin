using Healin.Domain.Interfaces.Repositories;
using Healin.Domain.Models;
using Healin.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Healin.Infrastructure.Data.Repositories
{
    public class DoctorRepository : BaseRespository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(HealinDbContext context) : base(context)
        {
        }

        public async Task<bool> CpfInUseAsync(Guid doctorId, string cpf)
        {
            return await _dbSet.AsNoTracking().AnyAsync(d => d.Id != doctorId && d.Cpf.Value == cpf);
        }

        public async Task<bool> CpfInUseAsync(string cpf)
        {
            return await _dbSet.AsNoTracking().AnyAsync(d => d.Cpf.Value == cpf);
        }

        public async Task<bool> CrmInUseAsync(Guid doctorId, string crm)
        {
            return await _dbSet.AsNoTracking().AnyAsync(d => d.Id != doctorId && d.Crm == crm);
        }

        public async Task<bool> CrmInUseAsync(string crm)
        {
            return await _dbSet.AsNoTracking().AnyAsync(d => d.Crm == crm);
        }

        public async Task<bool> EmailInUseAsync(Guid doctorId, string email)
        {
            return await _dbSet.AsNoTracking().AnyAsync(d => d.Id != doctorId && d.Email.Value == email);
        }

        public async Task<bool> EmailInUseAsync(string email)
        {
            return await _dbSet.AsNoTracking().AnyAsync(d => d.Email.Value == email);
        }

        public override async Task<Doctor> GetByIdAsync(Guid id, params string[] joins)
        {
            return await _dbSet.Includes(joins).FirstOrDefaultAsync(d => d.Id == id);
        }
    }

    public static class DoctorExtensions
    {
        public static IQueryable<Doctor> Includes(this IQueryable<Doctor> query, params string[] joins)
        {
            return joins.Aggregate(query, (current, @join) => @join switch
            {
                nameof(Doctor.Patients) => current.Include(d => d.Patients),
                nameof(Doctor.Address) => current.Include(d => d.Address),
                _ => current
            });
        }
    }
}
