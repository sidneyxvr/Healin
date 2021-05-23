using Healin.Shared.Utils;

namespace Healin.Domain.ValueObjects
{
    public class Email
    {
        public string Value { get; private set; }

        private Email(string value)
        {
            Value = value;
        }

        public static implicit operator Email(string email)
        {

            AssertionConcern.AssertArgumentNotEmpty(email, "O email é obrigatório");
            AssertionConcern.AssertArgumentMatches(CustomValidation.EmailRegularExpression, email, "Email inválido");

            return new Email(email);
        }

        public override string ToString() => Value;
    }
}
