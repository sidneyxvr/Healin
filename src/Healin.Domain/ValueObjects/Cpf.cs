using Healin.Shared.Utils;

namespace Healin.Domain.ValueObjects
{
    public class Cpf
    {
        public string Value { get; private set; }

        private Cpf(string value)
        {
            Value = value;
        }

        public static implicit operator Cpf(string cpf)
        {
            AssertionConcern.AssertArgumentNotEmpty(cpf, "O cpf é obrigatório");
            AssertionConcern.AssertArgumentTrue(boolValue: CustomValidation.ValidateCpf(cpf), "CPF inválido");

            return new Cpf(cpf);
        }

        public override string ToString() => Value;
    }
}
