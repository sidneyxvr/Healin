using Healin.Application.Interfaces;
using Healin.Application.Notifications;
using Healin.Application.Requests;
using Healin.Shared.Intefaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Healin.API.Controllers.V1
{
    [Route("doctor")]
    [ApiController]
    public class DoctorController : MainController
    {
        private readonly IDoctorService _doctorService;
        public DoctorController(
            IDoctorService doctorService, 
            INotifier notifier, 
            IAppUser appUser) : base(notifier, appUser)
        {
            _doctorService = doctorService;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            return CustomResponse(await _doctorService.GetPagedAsync(page, pageSize, search, filter, order));
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm]DoctorRequest doctorRequest)
        {
            await _doctorService.UpdateAsync(doctorRequest);

            return CustomResponse();
        }

        [HttpPut("address")]
        public async Task<IActionResult> UpdateAddress(AddressRequest addressRequest)
        {
            await _doctorService.UpdateAddressAsync(addressRequest);

            return CustomResponse();
        }

        [HttpGet("address")]
        public async Task<IActionResult> GetAddress()
        {
            return CustomResponse(await _doctorService.GetAddressAsync());
        }

        [HttpGet("paged-by-patient")]
        public async Task<IActionResult> GetPagedByPatient(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            return CustomResponse(await _doctorService.GetPagedByPatientAsync(page, pageSize, search, filter, order));
        }

        [HttpPut("image")]
        public async Task<IActionResult> UpdateImage(IFormFile image)
        {
            await _doctorService.UpdateImageAsync(image);

            return CustomResponse();
        }
    }
}
