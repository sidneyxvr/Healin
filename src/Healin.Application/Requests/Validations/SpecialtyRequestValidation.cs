using FluentValidation;

namespace Healin.Application.Requests.Validations
{
    public class SpecialtyRequestValidation : AbstractValidator<SpecialtyRequest>
    {
        public SpecialtyRequestValidation()
        {
            RuleFor(e => e.Name)
                .NotEmpty().WithMessage("O campo Nome é obrigatório")
                .MaximumLength(50).WithMessage("O campo Nome deve ter no máximo 50 caracteres");
        }
    }
}
