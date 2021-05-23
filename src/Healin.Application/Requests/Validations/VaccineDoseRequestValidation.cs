using FluentValidation;
using System;

namespace Healin.Application.Requests.Validations
{
    public class VaccineDoseRequestValidation : AbstractValidator<VaccineDoseRequest>
    {
        public VaccineDoseRequestValidation()
        {
            RuleFor(v => v.DoseDate)
                .NotEmpty().WithMessage("O campo Data da Dose é obrigatório")
                .GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Data da Dose inválida")
                .LessThan(DateTime.Now).WithMessage("Data da Dose inválida");

            RuleFor(d => d.DoseType)
                .NotEmpty().WithMessage("O campo Tipo da Dose é obrigatório")
                .IsInEnum().WithMessage("Tipo da Dose inválida");

            RuleFor(d => d.VaccineId)
                .NotEmpty().WithMessage("O campo Vacina é obrigatório");
        }
    }
}
