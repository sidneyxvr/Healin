using Healin.Application.Interfaces;
using Healin.Application.Notifications;
using Healin.Application.Requests;
using Healin.Shared.Intefaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Healin.API.Controllers.V1
{
    [Route("exam-result")]
    public class ExamResultController : MainController
    {
        private readonly IExamResultService _examResultService;
        public ExamResultController(
            IExamResultService examResultService, 
            INotifier notifier, 
            IAppUser appUser) : base(notifier, appUser)
        {
            _examResultService = examResultService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm]ExamResultRequest examResultRequest)
        {
            var files = Request.Form.Files;

            await _examResultService.AddAsync(examResultRequest, files.ElementAtOrDefault(0));

            return CustomResponse();
        }

        [HttpPut]
        public async Task<IActionResult> Update(ExamResultRequest examResultRequest)
        {
            await _examResultService.UpdateAsync(examResultRequest);

            return CustomResponse();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _examResultService.DeleteAsync(id);

            return CustomResponse();
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int page = 1, 
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            return CustomResponse(await _examResultService.GetPagedAsync(page, pageSize, search, filter, order));
        }

        [HttpGet("paged-by-patient")]
        public async Task<IActionResult> GetPaged(Guid patientId, int page = 1,
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            return CustomResponse(await _examResultService.GetPagedByPatientIdAsync(patientId, page, pageSize, search, filter, order));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return CustomResponse(await _examResultService.GetByIdAsync(id));
        }
    }
}
