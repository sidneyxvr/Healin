using Healin.Domain.Interfaces.Repositories.ReadOnly;
using Healin.Domain.Models;
using Healin.Infrastructure.Data.Context;
using Healin.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healin.Infrastructure.Data.Repositories.ReadOnly
{
    public class PrescriptionReadOnlyRepository : BaseReadOnlyRepository<Prescription>, IPrescriptionReadOnlyRepository
    {
        public PrescriptionReadOnlyRepository(HealinDbContext context) : base(context)
        {
        }

        public Task<IList<Prescription>> GetAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedListDTO<Prescription>> GetPagedByPatientAsync(Guid patientId, int page = 1, 
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var consulta = _dbSet.AsNoTrackingWithIdentityResolution().Where(prescription => prescription.PatientId == patientId);
            consulta = FilterOut(consulta, filter);
            consulta = Search(consulta, search);
            var ordenado = Order(consulta, order);
            ordenado = Page(ordenado, page, pageSize);
            return new PagedListDTO<Prescription>
            (
                await ordenado.Include(prescription => prescription.Specialty).ToListAsync(),
                await consulta.CountAsync()
            );
        }

        protected override IQueryable<Prescription> FilterOut(IQueryable<Prescription> query, string filter)
        {
            return query;
        }

        protected override IQueryable<Prescription> Order(IQueryable<Prescription> query, string order)
        {
            var _order = order?.Trim();

            return _order switch
            {
                "description" => query.OrderBy(examResult => examResult.Description),
                "specialty" => query.OrderBy(examResult => examResult.Specialty.Name),
                "prescriptionDate" => query.OrderBy(examType => examType.PrescriptionDate),
                _ => query.OrderBy(examType => examType.Created),
            };
        }

        protected override IQueryable<Prescription> Search(IQueryable<Prescription> query, string search)
        {
            return string.IsNullOrWhiteSpace(search) ?
                query :
                query.Where(examResult => examResult.Description.Contains(search) || 
                examResult.Specialty.Name.Contains(search));
        }
    }
}
