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
    public class SpecialtyAppService : BaseAppService, ISpecialtyService
    {
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly ISpecialtyReadOnlyRepository _specialtyReadOnlyRepository;

        public SpecialtyAppService(
            ISpecialtyRepository specialtyRepository,
            ISpecialtyReadOnlyRepository specialtyReadOnlyRepository,
            INotifier notifier, IMapper mapper) : base(notifier, mapper)
        {
            _specialtyRepository = specialtyRepository;
            _specialtyReadOnlyRepository = specialtyReadOnlyRepository;
        }

        public async Task AddAsync(SpecialtyRequest specialtyRequest)
        {
            if (!ExecuteValidation(new SpecialtyRequestValidation(), specialtyRequest))
            {
                return;
            }

            var specialty = Mapper.Map<Specialty>(specialtyRequest);

            await _specialtyRepository.AddAsync(specialty);
            await _specialtyRepository.UnitOfWork.CommitAsync();
        }

        public async Task<IList<SelectItem<Guid>>> GetAsync()
        {
            return Mapper.Map<List<SelectItem<Guid>>>(await _specialtyReadOnlyRepository.GetAsync());
        }

        public async Task<SpecialtyResponse> GetByIdAsync(Guid id)
        {
            return Mapper.Map<SpecialtyResponse>(await _specialtyRepository.GetByIdAsync(id));
        }

        public async Task<PagedListDTO<SpecialtyResponse>> GetPagedAsync(int page = 1, 
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var specialties = await _specialtyReadOnlyRepository.GetPagedAsync(page, pageSize, search, filter, order);

            return new PagedListDTO<SpecialtyResponse>
            (
                Mapper.Map<List<SpecialtyResponse>>(specialties.Collection),
                specialties.Amount
            );
        }

        public async Task DeleteAsync(SpecialtyRequest specialtyRequest)
        {
            var specialty = await _specialtyRepository.GetByIdAsync(specialtyRequest.Id);

            if (specialty is null)
            {
                Notify("Especialidade não encontrada");
                return;
            }

            specialty.Delete();

            await _specialtyRepository.UpdateAsync(specialty);
            await _specialtyRepository.UnitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(SpecialtyRequest specialtyRequest)
        {
            if (!ExecuteValidation(new SpecialtyRequestValidation(), specialtyRequest))
            {
                return;
            }

            var specialty = await _specialtyRepository.GetByIdAsync(specialtyRequest.Id);

            if(specialty is null)
            {
                Notify("Especialidade não encontrada");
                return;
            }

            specialty.Update(specialtyRequest.Name);
            if (specialtyRequest.IsActive)
            {
                specialty.Enable();
            }
            else
            {
                specialty.Disable();
            }
            
            await _specialtyRepository.UpdateAsync(specialty);
            await _specialtyRepository.UnitOfWork.CommitAsync();
        }
    }
}
