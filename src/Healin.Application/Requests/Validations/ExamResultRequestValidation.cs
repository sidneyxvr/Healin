using FluentValidation;
using System;

namespace Healin.Application.Requests.Validations
{
    public class ExamResultRequestValidation : AbstractValidator<ExamResultRequest>
    {
        public ExamResultRequestValidation()
        {
            RuleFor(e => e.Description)
                .NotEmpty().WithMessage("O campo Descrição é obrigatório")
                .MaximumLength(100).WithMessage("O campo Descrição deve ter no máximo 100 caracteres");

            RuleFor(e => e.ExamDate)
                .NotEmpty().WithMessage("O campo Data do Exame é obrigatório")
                .GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Data do Exame inválida")
                .LessThan(DateTime.Now).WithMessage("Data do Exame inválida");

            RuleFor(e => e.ExamTypeIds)
                .NotEmpty().WithMessage("O campo Tipos do Emaxe é obrigatório");
        }
    }
}
