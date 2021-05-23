using Healin.Application.Interfaces;
using Healin.Application.Notifications;
using Healin.Application.Requests;
using Healin.Shared.Intefaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Healin.API.Controllers.V1
{
    [Route("specialty")]
    public class SpecialtyController : MainController
    {
        private readonly ISpecialtyService _specialtyService;
        public SpecialtyController(
            ISpecialtyService specialtyService, 
            INotifier notifier, 
            IAppUser appUser) : base(notifier, appUser)
        {
            _specialtyService = specialtyService;
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(int page = 1, int pageSize = 10, string search = "", string filter = "", string order = "")
        {
            return CustomResponse(await _specialtyService.GetPagedAsync(page, pageSize, search, filter, order));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return CustomResponse(await _specialtyService.GetAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return CustomResponse(await _specialtyService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Add(SpecialtyRequest specialtyRequest)
        {
            await _specialtyService.AddAsync(specialtyRequest);

            return CustomResponse();
        }

        [HttpPut]
        public async Task<IActionResult> Update(SpecialtyRequest specialtyRequest)
        {
            await _specialtyService.UpdateAsync(specialtyRequest);

            return CustomResponse();
        }
    }
}
