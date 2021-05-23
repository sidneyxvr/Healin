using FluentValidation;

namespace Healin.Application.Requests.Validations
{
    public class ExamTypeRequestValidation : AbstractValidator<ExamTypeRequest>
    {
        public ExamTypeRequestValidation()
        {
            RuleFor(e => e.Name)
                .NotEmpty().WithMessage("O campo Nome é obrigatório")
                .MaximumLength(50).WithMessage("O campo Nome deve ter no máximo 50 caracteres");

            RuleFor(e => e.ExamId)
                .NotEmpty().WithMessage("O campo Exame é obrigatório");
        }
    }
}
