using Bogus;
using Bogus.Extensions.Brazil;
using Healin.Domain.ValueObjects;
using Healin.Shared.Exceptions;
using Xunit;

namespace Healin.Domain.Test
{
    public class CpfTest
    {
        [Trait("CPF", "Invalid")]
        [Theory(DisplayName = "Create cpf with invalid value")]
        [InlineData("teste@gmail.com")]
        [InlineData("78913897058")]
        [InlineData("85999887766")]
        [InlineData("95793326064")]
        [InlineData("doctorcpf")]
        [InlineData("invaliddocument")]
        [InlineData("00000000000")]
        [InlineData("11111122233")]
        [InlineData("957.933.260-65")]
        public void Cpf_NewCpf_ShouldBeInvalid(string cpf)
        {
            //Arrange 
            Cpf _cpf = null;

            // Act
            var result = Assert.Throws<DomainException>(() => _cpf = cpf);

            //Assert
            Assert.Equal("CPF inválido", result.Message);
        }

        [Trait("CPF", "Valid")]
        [Fact(DisplayName = "Create cpf with valid value")]
        public void Cpf_NewCpf_ShouldBeValid()
        {
            //Arrange 
            Cpf _cpf = null;
            var cpf = new Faker().Person.Cpf(false);

            // Act
            var result = Record.Exception(() => _cpf = cpf);

            //Assert
            Assert.Null(result);
        }
    }
}
