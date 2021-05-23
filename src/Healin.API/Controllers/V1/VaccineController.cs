using Healin.Application.Interfaces;
using Healin.Application.Notifications;
using Healin.Application.Requests;
using Healin.Shared.Intefaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Healin.API.Controllers.V1
{
    [Route("vaccine")]
    [ApiController]
    public class VaccineController : MainController
    {
        private readonly IVaccineService _vaccineService;
        public VaccineController(
            IVaccineService vaccineService, 
            INotifier notifier, 
            IAppUser appUser) : base(notifier, appUser)
        {
            _vaccineService = vaccineService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(VaccineRequest vaccineRequest)
        {
            await _vaccineService.AddAsync(vaccineRequest);

            return CustomResponse();
        }

        [HttpPut]
        public async Task<IActionResult> Update(VaccineRequest vaccineRequest)
        {
            await _vaccineService.UpdateAsync(vaccineRequest);

            return CustomResponse();
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            return CustomResponse(await _vaccineService.GetPagedAsync(page, pageSize, search, filter, order));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return CustomResponse(await _vaccineService.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return CustomResponse(await _vaccineService.GetByIdAsync(id));
        }
    }
}
