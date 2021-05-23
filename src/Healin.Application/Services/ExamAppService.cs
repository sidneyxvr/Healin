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
    public class ExamAppService : BaseAppService, IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IExamReadOnlyRepository _examReadOnlyRepository;

        public ExamAppService(
            IExamRepository examRepository,
            IExamReadOnlyRepository examReadOnlyRepository,
            INotifier notifier, IMapper mapper) : base(notifier, mapper)
        {
            _examRepository = examRepository;
            _examReadOnlyRepository = examReadOnlyRepository;
        }

        public async Task AddAsync(ExamRequest examRequest)
        {
            if (!ExecuteValidation(new ExamRequestValidation(), examRequest))
            {
                return;
            }

            var exam = Mapper.Map<Exam>(examRequest);

            await _examRepository.AddAsync(exam);
            await _examRepository.UnitOfWork.CommitAsync();
        }

        public async Task<PagedListDTO<ExamResponse>> GetPagedAsync(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var exams = await _examReadOnlyRepository.GetPagedAsync(page, pageSize, search, filter, order);
            return new PagedListDTO<ExamResponse>
            (
                Mapper.Map<ICollection<ExamResponse>>(exams.Collection),
                exams.Amount
            );
        }

        public async Task<ExamResponse> GetByIdAsync(Guid id)
        {
            return Mapper.Map<ExamResponse>(await _examRepository.GetByIdAsync(id));
        }

        public async Task DeleteAsync(ExamRequest examRequest)
        {
            var exam = await _examRepository.GetByIdAsync(examRequest.Id);

            if(exam is null)
            {
                Notify("Exame não encontrado");
                return;
            }

            exam.Delete();

            await _examRepository.UpdateAsync(exam);
            await _examRepository.UnitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(ExamRequest examRequest)
        {
            if (!ExecuteValidation(new ExamRequestValidation(), examRequest))
            {
                return;
            }

            var exam = await _examRepository.GetByIdAsync(examRequest.Id);
            if (exam is null)
            {
                Notify("Exame não encontrado");
                return;
            }

            exam.Update(examRequest.Name);

            if (examRequest.IsActive)
            {
                exam.Enable();
            }
            else
            {
                exam.Disable();
            }
            

            await _examRepository.UpdateAsync(exam);
            await _examRepository.UnitOfWork.CommitAsync();
        }

        public async Task<List<SelectItem<Guid>>> GetAsync()
        {
            return Mapper.Map<List<SelectItem<Guid>>>(await _examReadOnlyRepository.GetAsync());
        }
    }
}
