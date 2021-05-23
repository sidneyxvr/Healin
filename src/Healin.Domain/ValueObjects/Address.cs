using Healin.Shared.Utils;

namespace Healin.Domain.ValueObjects
{
    public class Address
    {
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string District { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public string PostalCode { get; private set; }
        public string Complement { get; private set; }

        protected Address() { }

        public Address(string street, string number, string district, string city, string state, string postalCode, string complement)
        {
            AssertionConcern.AssertArgumentNotEmpty(street, "A rua é obrigatória");
            AssertionConcern.AssertArgumentLength(street, 50, "A rua deve ter no máximo 50 caracteres");

            AssertionConcern.AssertArgumentLength(number, 5, "O número deve ter no máximo 5 caracteres");

            AssertionConcern.AssertArgumentNotEmpty(district, "O bairro é obrigatório");
            AssertionConcern.AssertArgumentLength(district, 50, "O bairro deve ter no máximo 50 caracteres");

            AssertionConcern.AssertArgumentNotEmpty(city, "A cidade é obrigatória");
            AssertionConcern.AssertArgumentLength(city, 40, "A cidade deve ter no máximo 40 caracteres");

            AssertionConcern.AssertArgumentNotEmpty(state, "O estado é obrigatório");
            AssertionConcern.AssertArgumentExactLength(state, 2, "Estado inválido");

            AssertionConcern.AssertArgumentNotEmpty(postalCode, "O cep é obrigatória");
            AssertionConcern.AssertArgumentLength(postalCode, 8, "Cep inválido");

            AssertionConcern.AssertArgumentLength(complement, 50, "O complemento deve ter no máximo 50 caracteres");

            Street = street;
            Number = number;
            District = district;
            City = city;
            State = state;
            Country = "Brasil";
            PostalCode = postalCode;
            Complement = complement;
        }

        public override string ToString() => $"{Street}, {Number} - {District}, {City} - {State}";
    }
}
