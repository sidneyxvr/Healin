using AutoMapper;
using Healin.Application.Interfaces;
using Healin.Application.Notifications;
using Healin.Application.Requests;
using Healin.Application.Requests.Validations;
using Healin.Application.Responses;
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
    public class ExamResultAppService : BaseAppService, IExamResultService
    {
        private readonly IAppUser _appUser;
        private readonly IFileStorage _fileStorage;
        private readonly IPatientRepository _patientRepository;
        private readonly IExamTypeRepository _examTypeRepository;
        private readonly IExamResultRepository _examResultRepository;
        private readonly IExamResultReadOnlyRepository _examResultReadOnlyRepository;

        public ExamResultAppService(
            IAppUser appUser,
            IFileStorage fileStorage,
            IPatientRepository patientRepository,
            IExamTypeRepository examTypeRepository,
            IExamResultRepository examResultRepository,
            IExamResultReadOnlyRepository examResultReadOnlyRepository,
            INotifier notifier, IMapper mapper) : base(notifier, mapper)
        {
            _appUser = appUser;
            _fileStorage = fileStorage;
            _patientRepository = patientRepository;
            _examTypeRepository = examTypeRepository;
            _examResultRepository = examResultRepository;
            _examResultReadOnlyRepository = examResultReadOnlyRepository;
        }

        public async Task AddAsync(ExamResultRequest examResultRequest, IFormFile file)
        {
            if (!ExecuteValidation(new ExamResultRequestValidation(), examResultRequest))
            {
                return;
            }

            if (file is null || file.Length == 0)
            {
                Notify("Selecione um arquivo");
                return;
            }

            var request = examResultRequest;

            var examResult = new ExamResult(request.Description, request.ExamDate, request.ExamId, _appUser.UserId);
            
            var storageResult = await _fileStorage.SaveFile(file);

            if (!storageResult.Succeeded)
            {
                storageResult.Errors.ToList().ForEach(error => Notify(error));
                return;
            }

            var examTypes = await _examTypeRepository.GetByIdsAsync(examResultRequest.ExamTypeIds.ToArray());

            examResult.AddFilePath(storageResult.Result);
            examResult.UpdateExamTypes(examTypes);

            await _examResultRepository.AddAsync(examResult);
            await _examResultRepository.UnitOfWork.CommitAsync();
        }

        public async Task<PagedListDTO<ExamResultResponse>> GetPagedAsync(int page = 1, 
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var patientId = _appUser.UserId;

            var examResults = await _examResultReadOnlyRepository.GetPagedByPatientIdAsync(patientId, page, pageSize, search, filter, order);
            return new PagedListDTO<ExamResultResponse>
            (
                Mapper.Map<ICollection<ExamResultResponse>>(examResults.Collection),
                examResults.Amount
            );
        }

        public async Task<ExamResultResponse> GetByIdAsync(Guid id)
        {
            return Mapper.Map<ExamResultResponse>(await _examResultRepository.GetByIdAsync(id, nameof(ExamResult.Exam), nameof(ExamResult.ExamTypes)));
        }

        public async Task DeleteAsync(Guid id)
        {
            var examResult = await _examResultRepository.GetByIdAsync(id);

            if(examResult is null)
            {
                Notify("Resultado de exame não encontrado");
                return;
            }

            await _fileStorage.RemoveFile(examResult.FilePath.ToString());

            await _examResultRepository.RemoveAsync(examResult);
            await _examResultRepository.UnitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(ExamResultRequest examResultRequest)
        {
            if (!ExecuteValidation(new ExamResultRequestValidation(), examResultRequest))
            {
                return;
            }

            var examResult = await _examResultRepository.GetByIdAsync(examResultRequest.Id, nameof(ExamResult.Exam), nameof(ExamResult.ExamTypes));
            
            examResult.Update(examResultRequest.Description, examResultRequest.ExamDate, examResultRequest.ExamId);

            var examTypes = await _examTypeRepository.GetByIdsAsync(examResultRequest.ExamTypeIds.ToArray());

            examResult.UpdateExamTypes(examTypes);

            await _examResultRepository.UpdateAsync(examResult);
            await _examResultRepository.UnitOfWork.CommitAsync();
        }

        public async Task<PagedListDTO<ExamResultResponse>> GetPagedByPatientIdAsync(Guid patientId, 
            int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var doctorId = _appUser.UserId;

            var patient = await _patientRepository.GetByIdAsync(patientId, nameof(Patient.Doctors));

            if (patient.Doctors.All(d => d.Id != doctorId))
            {
                return new PagedListDTO<ExamResultResponse>(new List<ExamResultResponse>(), 0);
            }

            var examResults = await _examResultReadOnlyRepository.GetPagedByPatientIdAsync(patientId, page, pageSize, search, filter, order);

            return new PagedListDTO<ExamResultResponse>
            (
                Mapper.Map<ICollection<ExamResultResponse>>(examResults.Collection),
                examResults.Amount
            );
        }
    }
}
