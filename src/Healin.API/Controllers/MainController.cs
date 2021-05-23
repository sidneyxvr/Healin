using Healin.Application.Notifications;
using Healin.Shared.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Healin.API.Controllers
{
    [ApiController]
    [Authorize]
    public class MainController : ControllerBase
    {
        private readonly INotifier _notifier;

        protected Guid UserId { get; private set; }

        public MainController(INotifier notifier, IAppUser appUser)
        {
            _notifier = notifier;

            if (appUser is null)
            {
                throw new ArgumentNullException(nameof(appUser));
            }

            UserId = appUser.UserId;
        }

        protected bool IsValid()
        {
            return !_notifier.HasNotification();
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (!IsValid())
            {
                return BadRequest(_notifier.GetAll().Select(error => error.Message));
            }

            return Ok(result);
        }

        protected void NotifyError(string error)
        {
            _notifier.Handle(new Notification(error));
        }
    }
}
