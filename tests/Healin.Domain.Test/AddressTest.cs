using Bogus;
using Healin.Domain.ValueObjects;
using Healin.Shared.Exceptions;
using System;
using Xunit;

namespace Healin.Domain.Test
{
    public class AddressTest
    {
        private static Address GenerateAddress()
        {
            var address = new Faker<Address>("pt_BR")
                .CustomInstantiator(f => new Address(
                    f.Address.StreetName(),
                    new Random().Next(1, 99999).ToString(),
                    f.Address.SecondaryAddress(),
                    f.Address.City(),
                    f.Address.StateAbbr(),
                    f.Address.ZipCode("########"),
                    new string('c', new Random().Next(1, 50))
                ));

            return address;
        }

        [Trait("Address", "Valid")]
        [Fact]
        public void Address_NewAddress_ShouldBeValid()
        {
            //Arrange && Act
            var result = Record.Exception(() => GenerateAddress());

            //Assert
            Assert.Null(result);
        }

        [Trait("Address", "Invalid")]
        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void Address_NewAddress_InvalidStreet(string street)
        {
            //Arrange 
            var address = GenerateAddress();

            //Act
            var result = Assert.Throws<DomainException>(() => new Address(street, address.Number, address.District, address.State, address.City, address.PostalCode, address.Complement));

            //Assert
            Assert.Equal("A rua é obrigatória", result.Message);
        }

        [Trait("Address", "Invalid")]
        [Fact]
        public void Address_NewAddress_MaxLengthStreet()
        {
            //Arrange 
            var address = GenerateAddress();
            var street = new string('s', new Random().Next(51, 100));

            //Act
            var result = Assert.Throws<DomainException>(() => new Address(street, address.Number, address.District, address.State, address.City, address.PostalCode, address.Complement));

            //Assert
            Assert.Equal("A rua deve ter no máximo 50 caracteres", result.Message);
        }

        [Trait("Address", "Invalid")]
        [Fact]
        public void Address_NewAddress_MaxLengthNumber()
        {
            //Arrange 
            var address = GenerateAddress();
            var number = new Random().Next(100000, 999999999).ToString();

            //Act
            var result = Assert.Throws<DomainException>(() => new Address(address.Street, number, address.District, address.State, address.City, address.PostalCode, address.Complement));

            //Assert
            Assert.Equal("O número deve ter no máximo 5 caracteres", result.Message);
        }

        [Trait("Address", "Invalid")]
        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void Address_NewAddress_EmptyDistrict(string district)
        {
            //Arrange 
            var address = GenerateAddress();

            //Act
            var result = Assert.Throws<DomainException>(() => new Address(address.Street, address.Number, district, address.State, address.City, address.PostalCode, address.Complement));

            //Assert
            Assert.Equal("O bairro é obrigatório", result.Message);
        }

        [Trait("Address", "Invalid")]
        [Fact]
        public void Address_NewAddress_MaxLengthDiscrict()
        {
            //Arrange 
            var address = GenerateAddress();
            var district = new string('s', new Random().Next(51, 100));

            //Act
            var result = Assert.Throws<DomainException>(() => new Address(address.Street, address.Number, district, address.State, address.City, address.PostalCode, address.Complement));

            //Assert
            Assert.Equal("O bairro deve ter no máximo 50 caracteres", result.Message);
        }
        
        [Trait("Address", "Invalid")]
        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void Address_NewAddress_EmptyCity(string city)
        {
            //Arrange 
            var address = GenerateAddress();

            //Act
            var result = Assert.Throws<DomainException>(() => new Address(address.Street, address.Number, address.District, city, address.State, address.PostalCode, address.Complement));

            //Assert
            Assert.Equal("A cidade é obrigatória", result.Message);
        }

        [Trait("Address", "Invalid")]
        [Fact]
        public void Address_NewAddress_MaxLengthCity()
        {
            //Arrange 
            var address = GenerateAddress();
            var city = new string('s', new Random().Next(41, 100));

            //Act
            var result = Assert.Throws<DomainException>(() => new Address(address.Street, address.Number, address.District, city, address.State, address.PostalCode, address.Complement));

            //Assert
            Assert.Equal("A cidade deve ter no máximo 40 caracteres", result.Message);
        }

        [Trait("Address", "Invalid")]
        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(null)]
        public void Address_NewAddress_EmptyState(string state)
        {
            //Arrange 
            var address = GenerateAddress();

            //Act
            var result = Assert.Throws<DomainException>(() => new Address(address.Street, address.Number, address.District, address.City, state, address.PostalCode, address.Complement));

            //Assert
            Assert.Equal("O estado é obrigatório", result.Message);
        }

        [Trait("Address", "Invalid")]
        [Fact]
        public void Address_NewAddress_InvalidState()
        {
            //Arrange 
            var address = GenerateAddress();
            var state = new string('s', new Random().Next(3, 5));

            //Act
            var result = Assert.Throws<DomainException>(() => new Address(address.Street, address.Number, address.District, state, address.City, address.PostalCode, address.Complement));

            //Assert
            Assert.Equal("Estado inválido", result.Message);
        }
    }
}
