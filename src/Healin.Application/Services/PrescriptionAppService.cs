using AutoMapper;
using Healin.Application.Interfaces;
using Healin.Application.Notifications;
using Healin.Application.Requests;
using Healin.Application.Requests.Validations;
using Healin.Application.Responses;
using Healin.Domain.Enums;
using Healin.Domain.Interfaces.Repositories;
using Healin.Domain.Interfaces.Repositories.ReadOnly;
using Healin.Domain.Models;
using Healin.Infrastructure.Storage;
using Healin.Shared.DTOs;
using Healin.Shared.Intefaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Healin.Application.Services
{
    public class PrescriptionAppService : BaseAppService, IPrescriptionService
    {
        private readonly IAppUser _appUser;
        private readonly IFileStorage _fileStorage;
        private readonly IPrescriptionRepository _prescriptionRepository;
        private readonly IPrescriptionReadOnlyRepository _prescriptionReadOnlyRepository;
        public PrescriptionAppService(
            IAppUser appUser,
            IFileStorage fileStorage,
            IPrescriptionRepository prescriptionRepository,
            IPrescriptionReadOnlyRepository prescriptionReadOnlyRepository,
            INotifier notifier, 
            IMapper mapper) : base(notifier, mapper)
        {
            _appUser = appUser;
            _fileStorage = fileStorage;
            _prescriptionRepository = prescriptionRepository;
            _prescriptionReadOnlyRepository = prescriptionReadOnlyRepository;
        }

        public async Task AddAsync(PrescriptionRequest prescriptionRequest, IFormFile file)
        {
            if (file is null || file.Length == 0)
            {
                Notify("Selecione um arquivo");
                return;
            }

            if (!ExecuteValidation(new PrescriptionRequestValidation(), prescriptionRequest))
            {
                return;
            }

            var storageResult = await _fileStorage.SaveFile(file);

            if (!storageResult.Succeeded)
            {
                storageResult.Errors.ToList().ForEach(Notify);
                return;
            }

            var prescription = Prescription.New(prescriptionRequest.Description, prescriptionRequest.PrescriptionDate, 
                prescriptionRequest.SpecialtyId, prescriptionRequest.PrescriptionType, _appUser.UserId);
            prescription.AddFilePath(storageResult.Result);

            await _prescriptionRepository.AddAsync(prescription);
            await _prescriptionRepository.UnitOfWork.CommitAsync();
        }

        public async Task<PrescriptionResponse> GetByIdAsync(Guid id)
        {
            return Mapper.Map<PrescriptionResponse>(await _prescriptionRepository.GetByIdAsync(id));
        }

        public async Task<PagedListDTO<PrescriptionResponse>> GetPagedAsync(int page = 1, 
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var prescriptions = await _prescriptionReadOnlyRepository.GetPagedByPatientAsync(_appUser.UserId, page, pageSize, search, filter, order);

            return new PagedListDTO<PrescriptionResponse> (
                Mapper.Map<ICollection<PrescriptionResponse>>(prescriptions.Collection),
                prescriptions.Amount
            );
        }

        public Task<IEnumerable<SelectItem<string>>> GetPrescriptionTypesAsync()
        {
            return Task.FromResult(
                Enum.GetValues(typeof(PrescriptionType))
                .Cast<PrescriptionType>()
                .Select(p => new SelectItem<string>
                { 
                    Text = p.GetDescription(),
                    Value = ((int)p).ToString()
                }));
        }

        public async Task DeleteAsync(Guid id)
        {
            var prescription = await _prescriptionRepository.GetByIdAsync(id);

            if (prescription is null)
            {
                Notify("Receituário não encontrado");
                return;
            }

            await _fileStorage.RemoveFile(prescription.FilePath.ToString());

            await _prescriptionRepository.RemoveAsync(prescription);
            await _prescriptionRepository.UnitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(PrescriptionRequest prescriptionRequest)
        {
            if (!ExecuteValidation(new PrescriptionRequestValidation(), prescriptionRequest))
            {
                return;
            }

            var prescription = await _prescriptionRepository.GetByIdAsync(prescriptionRequest.Id);

            if(prescription is null)
            {
                Notify("Receituário não encontrado");
                return;
            }

            prescription.Update(
                prescriptionRequest.Description, prescriptionRequest.PrescriptionDate, 
                prescriptionRequest.SpecialtyId, prescriptionRequest.PrescriptionType);

            await _prescriptionRepository.UpdateAsync(prescription);
            await _prescriptionRepository.UnitOfWork.CommitAsync();
        }
    }
}
