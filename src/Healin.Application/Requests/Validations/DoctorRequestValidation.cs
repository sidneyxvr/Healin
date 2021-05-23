using FluentValidation;
using Healin.Shared.Utils;
using System;

namespace Healin.Application.Requests.Validations
{
    public class DoctorRequestValidation : AbstractValidator<DoctorRequest>
    {
        public DoctorRequestValidation()
        {
            RuleFor(d => d.Name)
                .NotEmpty().WithMessage("O campo Nome é obrigatório")
                .MaximumLength(100).WithMessage("O campo Nome deve ter no máximo 100 caracteres");

            RuleFor(d => d.Cpf)
                .NotEmpty().WithMessage("O campo CPF é obrigatório")
                .Must(d => CustomValidation.ValidateCpf(d)).WithMessage("CPF inválido");

            RuleFor(d => d.Email)
                .NotEmpty().When(d => d.Id == Guid.Empty).WithMessage("O campo Email é obrigatório")
                .EmailAddress().WithMessage("Email inválido");

            RuleFor(d => d.Gender)
                .NotEmpty().WithMessage("O campo Sexo é obrigatório")
                .IsInEnum().WithMessage("Sexo inválido");

            RuleFor(d => d.Phone)
                .Matches(CustomValidation.PhoneRegularExpression).WithMessage("Celular inválido");

            RuleFor(d => d.BirthDate)
                .NotEmpty().WithMessage("O campo Data de Nascimento é obrigatório")
                .GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Data de Nascimento inválida")
                .LessThan(DateTime.Now).WithMessage("Data de Nascimento inválida");
        }
    }
}
