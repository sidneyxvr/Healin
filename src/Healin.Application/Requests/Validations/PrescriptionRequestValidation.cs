using FluentValidation;
using System;

namespace Healin.Application.Requests.Validations
{
    public class PrescriptionRequestValidation : AbstractValidator<PrescriptionRequest>
    {
        public PrescriptionRequestValidation()
        {
            RuleFor(e => e.Description)
                .NotEmpty().WithMessage("O campo Descrição é obrigatório")
                .MaximumLength(100).WithMessage("O campo Descrição deve ter no máximo 100 caracteres");

            RuleFor(e => e.PrescriptionDate)
                .NotEmpty().WithMessage("O campo Data do Exame é obrigatório")
                .GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Data do Exame inválida")
                .LessThan(DateTime.Now).WithMessage("Data do Exame inválida");

            RuleFor(d => d.PrescriptionType)
                .NotEmpty().WithMessage("O campo Tipo de Prescrição é obrigatório")
                .IsInEnum().WithMessage("Tipo de Prescrição inválida");

            RuleFor(d => d.SpecialtyId)
                .NotEmpty().WithMessage("O campo Especialidade é obrigatório");
        }
    }
}
