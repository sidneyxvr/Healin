using FluentValidation;

namespace Healin.Application.Requests.Validations
{
    public class AddressRequestValidation : AbstractValidator<AddressRequest>
    {
        public AddressRequestValidation()
        {
            RuleFor(a => a.PostalCode)
                .NotEmpty().WithMessage("O campo CEP é obrigatório")
                .Length(8).WithMessage("CEP inválido");

            RuleFor(a => a.Street)
                .NotEmpty().WithMessage("O campo Rua é obrigatório")
                .MaximumLength(50).WithMessage("O campo Rua deve ter no máximo 50 caracteres");

            RuleFor(a => a.Number)
                .MaximumLength(5).WithMessage("O campo Número deve ter no máximo 5 caracteres");

            RuleFor(a => a.District)
                .NotEmpty().WithMessage("O campo Bairro é obrigatório")
                .MaximumLength(50).WithMessage("O campo Bairro deve ter no máximo 50 caracteres");

            RuleFor(a => a.City)
                .NotEmpty().WithMessage("O campo Cidade é obrigatório")
                .MaximumLength(40).WithMessage("O campo Cidade deve ter no máximo 40 caracteres");

            RuleFor(a => a.State)
                .NotEmpty().WithMessage("O campo Estado é obrigatório")
                .Length(40).WithMessage("Estado inválido");

            RuleFor(a => a.Complement)
                .Length(50).WithMessage("O campo Complemento deve ter no máximo 50 caracteres");
        }
    }
}
