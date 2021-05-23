using Healin.Application.Interfaces;
using Healin.Application.Notifications;
using Healin.Application.Requests;
using Healin.Shared.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Healin.API.Controllers.V1
{
    [Route("exam-type")]
    public class ExamTypeController : MainController
    {
        private readonly IExamTypeService _examTypeService;
        public ExamTypeController(
            IExamTypeService examTypeService, 
            INotifier notifier, 
            IAppUser appUser) : base(notifier, appUser)
        {
            _examTypeService = examTypeService;
        }

        [HttpGet("paged")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaged(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            return CustomResponse(await _examTypeService.GetPagedAsync(page, pageSize, search, filter, order));
        }

        [HttpGet("exam/{examId}")]
        public async Task<IActionResult> GetByExamId(Guid examId)
        {
            return CustomResponse(await _examTypeService.GetByExamIdAsync(examId));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return CustomResponse(await _examTypeService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(ExamTypeRequest examTypeRequest)
        {
            await _examTypeService.AddAsync(examTypeRequest);

            return CustomResponse();
        }

        [HttpPut]
        public async Task<IActionResult> Update(ExamTypeRequest examTypeRequest)
        {
            await _examTypeService.UpdateAsync(examTypeRequest);

            return CustomResponse();
        }
    }
}
