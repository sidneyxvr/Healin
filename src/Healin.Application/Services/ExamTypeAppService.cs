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
    public class ExamTypeAppService : BaseAppService, IExamTypeService
    {
        private readonly IExamTypeRepository _examTypeRepository;
        private readonly IExamTypeReadOnlyRepository _examTypeReadOnlyRepository;

        public ExamTypeAppService(
            IExamTypeRepository examTypeRepository,
            IExamTypeReadOnlyRepository examTypeReadOnlyRepository,
            INotifier notifier, IMapper mapper) : base(notifier, mapper)
        {
            _examTypeRepository = examTypeRepository;
            _examTypeReadOnlyRepository = examTypeReadOnlyRepository;
        }

        public async Task AddAsync(ExamTypeRequest examTypeRequest)
        {
            if (!ExecuteValidation(new ExamTypeRequestValidation(), examTypeRequest))
            {
                return;
            }

            var examType = Mapper.Map<ExamType>(examTypeRequest);

            await _examTypeRepository.AddAsync(examType);
            await _examTypeRepository.UnitOfWork.CommitAsync();
        }

        public async Task<PagedListDTO<ExamTypeResponse>> GetPagedAsync(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            var examTypes = await _examTypeReadOnlyRepository.GetPagedAsync(page, pageSize, search, filter, order);
            return new PagedListDTO<ExamTypeResponse>
            (
                Mapper.Map<ICollection<ExamTypeResponse>>(examTypes.Collection),
                examTypes.Amount
            );
        }

        public async Task<ExamTypeResponse> GetByIdAsync(Guid id)
        {
            return Mapper.Map<ExamTypeResponse>(await _examTypeRepository.GetByIdAsync(id));
        }

        public async Task DeleteAsync(ExamTypeRequest examTypeRequest)
        {
            var examType = await _examTypeRepository.GetByIdAsync(examTypeRequest.Id);

            if(examType is null)
            {
                Notify("Tipo de exame não encontrado");
                return;
            }

            examType.Delete();

            await _examTypeRepository.UpdateAsync(examType);
            await _examTypeRepository.UnitOfWork.CommitAsync();
        }

        public async Task UpdateAsync(ExamTypeRequest examTypeRequest)
        {
            if (!ExecuteValidation(new ExamTypeRequestValidation(), examTypeRequest))
            {
                return;
            }

            var examType = await _examTypeRepository.GetByIdAsync(examTypeRequest.Id);

            if (examType is null)
            {
                Notify("Tipo de exame não encontrado");
                return;
            }

            examType.Update(examTypeRequest.Name, examTypeRequest.ExamId);

            await _examTypeRepository.UpdateAsync(examType);
            await _examTypeRepository.UnitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<SelectItem<Guid>>> GetByExamIdAsync(Guid examId)
        {
            return Mapper.Map<IEnumerable<SelectItem<Guid>>>(await _examTypeReadOnlyRepository.GetByExamIdAsync(examId));
        }
    }
}
