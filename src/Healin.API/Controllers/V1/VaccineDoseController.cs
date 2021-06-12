using Healin.Application.Interfaces;
using Healin.Application.Notifications;
using Healin.Application.Requests;
using Healin.Shared.Intefaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Healin.API.Controllers.V1
{
    [Route("vaccine-dose")]
    [ApiController]
    public class VaccineDoseController : MainController
    {
        private readonly IVaccineDoseService _vaccineDoseService;
        public VaccineDoseController(
            IVaccineDoseService vaccineDoseService, 
            INotifier notifier, 
            IAppUser appUser) : base(notifier, appUser)
        {
            _vaccineDoseService = vaccineDoseService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(VaccineDoseRequest vaccineDoseRequest)
        {
            await _vaccineDoseService.AddAsync(vaccineDoseRequest);

            return CustomResponse();
        }

        [HttpGet("dose-types")]
        public async Task<IActionResult> GetVaccineType()
        {
            return CustomResponse(await _vaccineDoseService.GetDoseTypesAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return CustomResponse(await _vaccineDoseService.GetByLoggedPatientAsync());
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatientId(Guid patientId)
        {
            return CustomResponse(await _vaccineDoseService.GetByPatientIdAsync(patientId));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _vaccineDoseService.RemoveAsync(id);

            return CustomResponse();
        }
    }
}
