using Healin.Application.Interfaces;
using Healin.Application.Notifications;
using Healin.Application.Requests;
using Healin.Shared.Intefaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Healin.API.Controllers.V1
{
    [Route("patient")]
    public class PatientController : MainController
    {
        private readonly IPatientService _patientService;
        public PatientController(IPatientService patientService, 
            INotifier notifier, IAppUser appUser) : base(notifier, appUser)
        {
            _patientService = patientService;
        }

        [HttpPost("doctor")]
        public async Task<IActionResult> AddDoctorToMyDoctors(IdRequest request)
        {
            await _patientService.AddDoctorToMyDoctorsAsync(request.Id);

            return CustomResponse();
        }

        [HttpPut("address")]
        public async Task<IActionResult> UpdateAddress(AddressRequest addressRequest)
        {
            await _patientService.UpdateAddressAsync(addressRequest);

            return CustomResponse();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm]PatientRequest patientRequest)
        {
            await _patientService.UpdateAsync(patientRequest);

            return CustomResponse();
        }

        [HttpPut("image")]
        public async Task<IActionResult> UpdateImage(IFormFile image)
        {
            await _patientService.UpdateImageAsync(image);

            return CustomResponse();
        }

        [HttpGet("address")]
        public async Task<IActionResult> GetAddress()
        {
            return CustomResponse(await _patientService.GetAddressAsync());
        }

        [HttpDelete("doctor/{doctorId}")]
        public async Task<IActionResult> RemoveDoctorToMyDoctors(Guid doctorId)
        {
            await _patientService.RemoveDoctorFromMyDoctorsAsync(doctorId);

            return CustomResponse();
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int page = 1,
            int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            return CustomResponse(await _patientService.GetPagedByDoctorIdAsync(page, pageSize, search, filter, order));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return CustomResponse(await _patientService.GetByIdAsync(id));
        }
    }
}
