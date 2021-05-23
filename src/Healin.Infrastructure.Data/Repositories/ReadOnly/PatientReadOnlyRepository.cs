using Healin.Domain.Interfaces.Repositories.ReadOnly;
using Healin.Domain.Models;
using Healin.Domain.ValueObjects;
using Healin.Infrastructure.Data.Context;
using Healin.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healin.Infrastructure.Data.Repositories.ReadOnly
{
    public class PatientReadOnlyRepository : BaseReadOnlyRepository<Patient>, IPatientReadOnlyRepository
    {
        public PatientReadOnlyRepository(HealinDbContext context) : base(context)
        {
        }

        public Task<IList<Patient>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedListDTO<Patient>> GetPagedByDoctorIdAsync(Guid doctorId, int page = 1, 
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var query = _dbSet.AsNoTrackingWithIdentityResolution().Where(patient => patient.Doctors.Any(doctor => doctor.Id == doctorId));
            query = FilterOut(query, filter);
            query = Search(query, search);
            var ordered = Order(query, order);
            ordered = Page(ordered, page, pageSize);
            return new PagedListDTO<Patient>
            (
                await ordered.ToListAsync(),
                await query.CountAsync()
            );
        }

        public async Task<PagedListDTO<Patient>> GetPagedAsync(int page = 1, 
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var query = _dbSet.AsNoTrackingWithIdentityResolution();
            query = FilterOut(query, filter);
            query = Search(query, search);
            var ordered = Order(query, order);
            ordered = Page(ordered, page, pageSize);
            return new PagedListDTO<Patient>
            (
                await ordered.ToListAsync(),
                await query.CountAsync()
            );
        }

        protected override IQueryable<Patient> FilterOut(IQueryable<Patient> query, string filter)
        {
            return query;
        }

        protected override IQueryable<Patient> Order(IQueryable<Patient> query, string order)
        {
            var _order = order?.Trim();

            return _order switch
            {
                "name" => query.OrderBy(patient => patient.Name),
                "birthDate" => query.OrderBy(patient => patient.BirthDate),
                _ => query.OrderBy(examType => examType.Created),
            };
        }

        protected override IQueryable<Patient> Search(IQueryable<Patient> query, string search)
        {
            return string.IsNullOrWhiteSpace(search) ?
                query :
                query.Where(patient => patient.Name.Contains(search) || patient.Cpf.Value == search || patient.Email.Value == search);
        }

        public async Task<Address> GetAddressAsync(Guid patientId)
        {
            return await _dbSet.AsNoTracking().Where(p => p.Id == patientId).Select(p => p.Address).FirstOrDefaultAsync();
        }
    }
}
