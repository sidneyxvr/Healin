using Bogus;
using Healin.Domain.ValueObjects;
using Healin.Shared.Exceptions;
using Xunit;

namespace Healin.Domain.Test
{
    public class EmailTest
    {
        [Trait("Email", "Valid")]
        [Fact(DisplayName = "Create email with valid value")]
        public void Email_NewEmail_ShouldBeValid()
        {
            //Arrange
            var email = new Faker().Person.Email;
            Email _email;

            //Act
            var result = Record.Exception(() => _email = email);

            //Assert
            Assert.Null(result);
        }

        [Trait("Email", "Invalid")]
        [Theory(DisplayName = "Create email with valid value")]
        [InlineData("teste")]
        [InlineData("teste teste")]
        [InlineData("teste@teste")]
        [InlineData("123456789")]
        [InlineData("0000000000")]
        [InlineData("teste.com.br")]
        public void Email_NewEmail_ShouldBeInvalid(string email)
        {
            //Arrange
            Email _email;

            //Act
            var result = Assert.Throws<DomainException>(() => _email = email);

            //Assert
            Assert.Equal("Email inválido", result.Message);
        }
    }
}
