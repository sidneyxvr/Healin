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
    [Route("prescription")]
    [ApiController]
    public class PrescriptionController : MainController
    {
        private readonly IPrescriptionService _prescriptionService;
        public PrescriptionController(
            IPrescriptionService prescriptionService, 
            INotifier notifier, 
            IAppUser appUser) : base(notifier, appUser)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm]PrescriptionRequest prescriptionRequest)
        {
            var files = Request.Form.Files;

            await _prescriptionService.AddAsync(prescriptionRequest, files.ElementAtOrDefault(0));

            return CustomResponse();
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int page = 1,
           int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            return CustomResponse(await _prescriptionService.GetPagedAsync(page, pageSize, search, filter, order));
        }

        [HttpGet("paged-by-patient/{patientId}")]
        public async Task<IActionResult> GetPaged(Guid patientId, int page = 1,
           int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            return CustomResponse(await _prescriptionService.GetPagedByPatientIdAsync(patientId, page, pageSize, search, filter, order));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return CustomResponse(await _prescriptionService.GetByIdAsync(id));
        }

        [HttpGet("prescription-types")]
        public async Task<IActionResult> GetPrescriptionTypes()
        {
            return CustomResponse(await _prescriptionService.GetPrescriptionTypesAsync());
        }

        [HttpPut]
        public async Task<IActionResult> Update(PrescriptionRequest prescriptionRequest)
        { 
            await _prescriptionService.UpdateAsync(prescriptionRequest);

            return CustomResponse();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _prescriptionService.DeleteAsync(id);

            return CustomResponse();
        }
    }
}
