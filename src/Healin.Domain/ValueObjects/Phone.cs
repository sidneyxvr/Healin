using Healin.Shared.Utils;

namespace Healin.Domain.ValueObjects
{
    public class Phone
    {
        public string Value { get; private set; }

        private Phone(string value)
        {
            Value = value;
        }

        public static implicit operator Phone(string phone)
        {
            AssertionConcern.AssertArgumentMatches(CustomValidation.PhoneRegularExpression, phone, "Celular inválido");

            return new Phone(phone);
        }

        public override string ToString() => Value;
    }
}
