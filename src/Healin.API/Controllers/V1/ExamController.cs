using Healin.Application.Interfaces;
using Healin.Application.Notifications;
using Healin.Application.Requests;
using Healin.Shared.Intefaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Healin.API.Controllers.V1
{
    [Route("exam")]
    public class ExamController : MainController
    {
        private readonly IExamService _examService;
        public ExamController(
            IExamService examService, 
            INotifier notifier, 
            IAppUser appUser) : base(notifier, appUser)
        {
            _examService = examService;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            return CustomResponse(await _examService.GetPagedAsync(page, pageSize, search, filter, order));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return CustomResponse(await _examService.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return CustomResponse(await _examService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(ExamRequest examRequest)
        {
            await _examService.AddAsync(examRequest);

            return CustomResponse();
        }

        [HttpPut]
        public async Task<IActionResult> Update(ExamRequest examRequest)
        {
            await _examService.UpdateAsync(examRequest);

            return CustomResponse();
        }
    }
}
