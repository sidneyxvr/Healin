using Healin.Domain.ValueObjects;
using Healin.Shared.Exceptions;
using System;
using Xunit;

namespace Healin.Domain.Test
{
    public class PhoneTest
    {
        [Trait("Phone", "Valid")]
        [Fact(DisplayName = "Create phone with valid value")]
        public void Phone_NewPhone_ShouldBeValid()
        {
            //Arrange
            var phone = $"85{new Random().Next(960000000, 999999999)}";
            Phone _phone;

            //Act
            var result = Record.Exception(() => _phone = phone);

            //Assert
            Assert.Null(result);
        }

        [Trait("Phone", "Invalid")]
        [Theory(DisplayName = "Create phone with valid value")]
        [InlineData("teste@email.com")]
        [InlineData("85123456781")]
        [InlineData("00000000000")]
        [InlineData("teste teste")]
        public void Phone_NewPhone_ShouldBeInvalid(string phone)
        {
            //Arrange
            Phone _phone;

            //Act
            var result = Assert.Throws<DomainException>(() => _phone = phone);

            //Assert
            Assert.Equal("Celular inválido", result.Message);
        }
    }
}
