using Healin.Domain.Interfaces.Repositories.ReadOnly;
using Healin.Domain.Models;
using Healin.Domain.ValueObjects;
using Healin.Infrastructure.Data.Context;
using Healin.Shared.DTOs;
using Healin.Shared.Intefaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healin.Infrastructure.Data.Repositories.ReadOnly
{
    public class DoctorReadOnlyRepository : BaseReadOnlyRepository<Doctor>, IDoctorReadOnlyRepository
    {
        private readonly IAppUser _appUser;
        public DoctorReadOnlyRepository(HealinDbContext context, IAppUser appUser) : base(context)
        {
            _appUser = appUser;
        }

        public Task<IList<Doctor>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedListDTO<Doctor>> GetPagedByPatientIdAsync(Guid patientId, int page = 1,
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var consulta = _dbSet.AsNoTrackingWithIdentityResolution();
            consulta = FilterOut(consulta, filter);
            consulta = Search(consulta, search);
            var ordenado = Order(consulta, order);
            if (string.IsNullOrWhiteSpace(order))
            {
                ordenado = ordenado.OrderBy(doctor => !doctor.Patients.Any())
                    .ThenBy(doctor => doctor.Name);
            }
            ordenado = Page(ordenado, page, pageSize);
            return new PagedListDTO<Doctor>
            (
                await ordenado
                .Include(doctor => doctor.Patients.Where(patient => patient.Id == patientId))
                .ToListAsync(),
                await consulta.CountAsync()
            );
        }

        public async Task<PagedListDTO<Doctor>> GetPagedAsync(int page = 1, 
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var consulta = _dbSet.AsNoTrackingWithIdentityResolution();
            consulta = FilterOut(consulta, filter);
            consulta = Search(consulta, search);
            var ordenado = Order(consulta, order);
            ordenado = Page(ordenado, page, pageSize);
            return new PagedListDTO<Doctor>
            (
                await ordenado.ToListAsync(),
                await consulta.CountAsync()
            );
        }

        public async Task<PagedListDTO<Doctor>> GetPagedMarkedAsync(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var consulta = _dbSet.AsNoTrackingWithIdentityResolution();
            consulta = FilterOut(consulta, filter);
            consulta = Search(consulta, search);
            var ordenado = Order(consulta, order);
            ordenado = Page(ordenado, page, pageSize);
            return new PagedListDTO<Doctor>
            (
                await ordenado.Include(doctor => doctor.Patients.Where(patient => patient.Id == _appUser.UserId)).ToListAsync(),
                await consulta.CountAsync()
            );
        }

        protected override IQueryable<Doctor> FilterOut(IQueryable<Doctor> query, string filter)
        {
            return query;
        }

        protected override IQueryable<Doctor> Order(IQueryable<Doctor> query, string order)
        {
            var _order = order?.Trim();

            return _order switch
            {
                "name" => query.OrderBy(doctor => doctor.Name),
                "crm" => query.OrderBy(doctor => doctor.Crm),
                _ => query.OrderBy(doctor => doctor.Created),
            };
        }

        protected override IQueryable<Doctor> Search(IQueryable<Doctor> query, string search)
        {
            if (!string.IsNullOrWhiteSpace(search))
            {
                return query.Where(d => d.Name.Contains(search) || d.Crm == search);
            }

            return query;
        }

        public async Task<Address> GetAddressAsync(Guid doctorId)
        {
            return await _dbSet.AsNoTracking().Where(p => p.Id == doctorId).Select(p => p.Address).FirstOrDefaultAsync();
        }
    }
}
