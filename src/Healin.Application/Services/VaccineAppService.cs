using AutoMapper;
using Healin.Application.Interfaces;
using Healin.Application.Notifications;
using Healin.Application.Requests;
using Healin.Application.Requests.Validations;
using Healin.Application.Responses;
using Healin.Domain.Interfaces.Repositories;
using Healin.Domain.Interfaces.Repositories.ReadOnly;
using Healin.Domain.Models;
using Healin.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Application.Services
{
    public class VaccineAppService : BaseAppService, IVaccineService
    {
        private readonly IVaccineReadOnlyRepository _vaccineReadOnlyRepository;
        private readonly IVaccineRepository _vaccineRepository;
        public VaccineAppService(
            IVaccineReadOnlyRepository vaccineReadOnlyRepository,
            IVaccineRepository vaccineRepository,
            INotifier notifier, 
            IMapper mapper) : base(notifier, mapper)
        {
            _vaccineReadOnlyRepository = vaccineReadOnlyRepository;
            _vaccineRepository = vaccineRepository;
        }

        public async Task AddAsync(VaccineRequest vaccineRequest)
        {
            if(!ExecuteValidation(new VaccineRequestValidation(), vaccineRequest))
            {
                return;
            }

            var vaccine = Mapper.Map<Vaccine>(vaccineRequest);

            await _vaccineRepository.AddAsync(vaccine);
            await _vaccineRepository.UnitOfWork.CommitAsync();
        }

        public async Task<IList<SelectItem<Guid>>> GetAsync()
        {
            return Mapper.Map<IList<SelectItem<Guid>>>(await _vaccineReadOnlyRepository.GetAsync());
        }

        public async Task<VaccineResponse> GetByIdAsync(Guid id)
        {
            return Mapper.Map<VaccineResponse>(await _vaccineRepository.GetByIdAsync(id));
        }

        public async Task<PagedListDTO<VaccineResponse>> GetPagedAsync(int page = 1, 
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var examTypes = await _vaccineReadOnlyRepository.GetPagedAsync(page, pageSize, search, filter, order);
            return new PagedListDTO<VaccineResponse>
            (
                Mapper.Map<ICollection<VaccineResponse>>(examTypes.Collection),
                examTypes.Amount
            );
        }

        public async Task UpdateAsync(VaccineRequest vaccineRequest)
        {
            if (!ExecuteValidation(new VaccineRequestValidation(), vaccineRequest))
            {
                return;
            }

            var vaccine = await _vaccineRepository.GetByIdAsync(vaccineRequest.Id);

            if(vaccine is null)
            {
                Notify("Vacina não encontrada");
                return;
            }

            vaccine.Update(vaccineRequest.Name);

            if (vaccineRequest.IsActive)
            {
                vaccine.Enable();
            }
            else
            {
                vaccine.Disable();
            }

            await _vaccineRepository.UpdateAsync(vaccine);
            await _vaccineRepository.UnitOfWork.CommitAsync();
        }
    }
}
