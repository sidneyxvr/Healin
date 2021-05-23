using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Healin.Application.Notifications;
using Healin.Application.Requests;
using System;

namespace Healin.Application.Services
{
    public class BaseAppService
    {
        private readonly INotifier _notifier;
        protected readonly IMapper Mapper;

        public BaseAppService(INotifier notifier, IMapper mapper)
        {
            _notifier = notifier;
            Mapper = mapper;
        }

        protected void Notify(string message)
        {
            _notifier.Handle(new Notification(message));
        }

        private void Notify(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notify(error.ToString());
            }
        }

        protected bool ExecuteValidation<TValidation, TRequest>(TValidation validation, TRequest request)
            where TValidation : AbstractValidator<TRequest>
            where TRequest : RequestBase
        {
            if(validation is null)
            {
                throw new ArgumentNullException(nameof(validation));
            }

            var validator = validation.Validate(request);

            if (validator.IsValid)
            {
                return true;
            }

            Notify(validator);

            return false;
        }
    }
}
