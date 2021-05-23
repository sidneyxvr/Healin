using Bogus;
using Bogus.DataSets;
using Bogus.Extensions.Brazil;
using Healin.Domain.Enums;
using Healin.Domain.Models;
using Healin.Shared.Exceptions;
using System;
using Xunit;

namespace Healin.Domain.Test
{
    public class DoctorTest
    {
        public static readonly object[][] InvalidDates =
        {
            new object[] { DateTime.Now.AddYears(-101) },
            new object[] { DateTime.Now.AddDays(1) },
            new object[] { DateTime.MinValue },
            new object[] { DateTime.MaxValue },
            new object[] { DateTime.Now.AddYears(-17) }
        };

        public static readonly object[][] InvalidImagePaths =
        {
            new object[] { $"{Guid.NewGuid()}.mp3" },
            new object[] { $"{Guid.NewGuid()}.mp4" },
            new object[] { $"{Guid.NewGuid()}.pdf" },
            new object[] { $"{Guid.NewGuid()}.docx" },
            new object[] { $"{Guid.NewGuid()}.xls" },
        };

        public static readonly object[][] ValidImagePaths =
        {
            new object[] { $"{Guid.NewGuid()}.jpeg" },
            new object[] { $"{Guid.NewGuid()}.jpg" },
            new object[] { $"{Guid.NewGuid()}.PNG" },
        };

        public static Doctor GenerateValidDoctor()
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            var doctorFake = new Faker<Doctor>("pt_BR")
                .CustomInstantiator(f => Doctor.New(
                    f.Random.Guid(),
                    f.Name.FullName(genero),
                    f.Person.Cpf(false),
                    f.Internet.Email(f.Name.FullName(genero)),
                    genero == Name.Gender.Female ? Gender.Female : Gender.Male,
                    string.Concat("85", f.Random.Number(960000000, 999999999).ToString()),
                    f.Date.Past(80, DateTime.Now.AddYears(-18)),
                    f.Random.Number(1, 1000000000).ToString()));

            return doctorFake;
        }

        [Trait("Doctor", "Valid")]
        [Fact(DisplayName = "Create a valid doctor")]
        public void Doctor_CreateDoctor_ShouldBeValid()
        {
            //Arrange
            var doctor = GenerateValidDoctor();

            //Act
            var result = Record.Exception(() => Doctor.New(Guid.NewGuid(), doctor.Name, doctor.Cpf.Value,
                doctor.Email.Value, doctor.Gender, doctor.Phone.Value, doctor.BirthDate, doctor.Crm));

            //Assert
            Assert.Null(result);
        }

        [Trait("Doctor", "Invalid")]
        [Theory(DisplayName = "Create doctor with required name")]
        [InlineData("")]
        [InlineData("      ")]
        [InlineData(null)]
        public void Doctor_CreateDoctor_RequiredName(string name)
        {
            //Arrange
            var doctor = GenerateValidDoctor();

            //Act
            var result = Assert.Throws<DomainException>(() => Doctor.New(Guid.NewGuid(), name, doctor.Cpf.Value, 
                doctor.Email.Value, doctor.Gender, doctor.Phone.Value, doctor.BirthDate, doctor.Crm));

            //Assert
            Assert.Equal("O nome é obrigatório", result.Message);
        }

        [Trait("Doctor", "Invalid")]
        [Fact(DisplayName = "Create doctor with max length name")]
        public void Doctor_CreateDoctor_MaxLengthName()
        {
            //Arrange
            var doctor = GenerateValidDoctor();
            var name = new string('t', new Random().Next(51, 100));

            //Act
            var result = Assert.Throws<DomainException>(() => Doctor.New(Guid.NewGuid(), name, doctor.Cpf.Value,
                doctor.Email.Value, doctor.Gender, doctor.Phone.Value, doctor.BirthDate, doctor.Crm));

            //Assert
            Assert.Equal("O nome deve ter no máximo 50 caracteres", result.Message);
        }

        [Trait("Doctor", "Invalid")]
        [Theory(DisplayName = "Create doctor with invalid gender")]
        [InlineData((Gender)0)]
        [InlineData((Gender)4)]
        public void Doctor_CreateDoctor_InvalidGender(Gender gender)
        {
            //Arrange
            var doctor = GenerateValidDoctor();

            //Act
            var result = Assert.Throws<DomainException>(() => Doctor.New(Guid.NewGuid(), doctor.Name, doctor.Cpf.Value,
                doctor.Email.Value, gender, doctor.Phone.Value, doctor.BirthDate, doctor.Crm));

            //Assert
            Assert.Equal("Sexo inválido", result.Message);
        }

        [Trait("Doctor", "Invalid")]
        [Theory(DisplayName = "Create doctor with invalid birth date")]
        [MemberData(nameof(InvalidDates))]
        public void Doctor_CreateDoctor_InvalidBirthDate(DateTime birthDate)
        {
            //Arrange
            var doctor = GenerateValidDoctor();

            //Act
            var result = Assert.Throws<DomainException>(() => Doctor.New(Guid.NewGuid(), doctor.Name, doctor.Cpf.Value,
                doctor.Email.Value, doctor.Gender, doctor.Phone.Value, birthDate, doctor.Crm));

            //Assert
            Assert.Equal("Data de Nascimento inválida", result.Message);
        }

        [Trait("Doctor", "Invalid")]
        [Theory(DisplayName = "Create doctor with required crm")]
        [InlineData("")]
        [InlineData("      ")]
        [InlineData(null)]
        public void Doctor_CreateDoctor_RequiredCrm(string crm)
        {
            //Arrange
            var doctor = GenerateValidDoctor();

            //Act
            var result = Assert.Throws<DomainException>(() => Doctor.New(Guid.NewGuid(), doctor.Name, doctor.Cpf.Value,
                doctor.Email.Value, doctor.Gender, doctor.Phone.Value, doctor.BirthDate, crm));

            //Assert
            Assert.Equal("O crm é obrigatório", result.Message);
        }


        [Trait("Doctor", "Invalid")]
        [Fact(DisplayName = "Create doctor with max length crm")]
        public void Doctor_CreateDoctor_MaxLengthCrm()
        {
            //Arrange
            var doctor = GenerateValidDoctor();
            var crm = new string('c', new Random().Next(11, 20));

            //Act
            var result = Assert.Throws<DomainException>(() => Doctor.New(Guid.NewGuid(), doctor.Name, doctor.Cpf.Value,
                doctor.Email.Value, doctor.Gender, doctor.Phone.Value, doctor.BirthDate, crm));

            //Assert
            Assert.Equal("O crm deve ter no máximo 10 caracteres", result.Message);
        }

        [Trait("Doctor", "Valid")]
        [Fact(DisplayName = "Update a valid doctor")]
        public void Doctor_UpdateDoctor_ShouldBeValid()
        {
            //Arrange
            var doctor = GenerateValidDoctor();

            //Act
            var result = Record.Exception(() => doctor.Update(doctor.Name, doctor.Cpf.Value,
                doctor.Gender, doctor.Phone.Value, doctor.BirthDate, doctor.Crm));

            //Assert
            Assert.Null(result);
        }

        [Trait("Doctor", "Invalid")]
        [Theory(DisplayName = "Update doctor with required name")]
        [InlineData("")]
        [InlineData("      ")]
        [InlineData(null)]
        public void Doctor_UpdateDoctor_RequiredName(string name)
        {
            //Arrange
            var doctor = GenerateValidDoctor();

            //Act
            var result = Assert.Throws<DomainException>(() => doctor.Update(name, doctor.Cpf.Value,
                doctor.Gender, doctor.Phone.Value, doctor.BirthDate, doctor.Crm));

            //Assert
            Assert.Equal("O nome é obrigatório", result.Message);
        }

        [Trait("Doctor", "Invalid")]
        [Fact(DisplayName = "Update doctor with max length name")]
        public void Doctor_UpdateDoctor_MaxLengthName()
        {
            //Arrange
            var doctor = GenerateValidDoctor();
            var name = new string('t', new Random().Next(51, 100));

            //Act
            var result = Assert.Throws<DomainException>(() => doctor.Update(name, doctor.Cpf.Value,
                doctor.Gender, doctor.Phone.Value, doctor.BirthDate, doctor.Crm));

            //Assert
            Assert.Equal("O nome deve ter no máximo 50 caracteres", result.Message);
        }

        [Trait("Doctor", "Invalid")]
        [Theory(DisplayName = "Update doctor with invalid gender")]
        [InlineData((Gender)0)]
        [InlineData((Gender)4)]
        public void Doctor_UpdateDoctor_InvalidGender(Gender gender)
        {
            //Arrange
            var doctor = GenerateValidDoctor();

            //Act
            var result = Assert.Throws<DomainException>(() => doctor.Update(doctor.Name, doctor.Cpf.Value,
                gender, doctor.Phone.Value, doctor.BirthDate, doctor.Crm));

            //Assert
            Assert.Equal("Sexo inválido", result.Message);
        }

        [Trait("Doctor", "Invalid")]
        [Theory(DisplayName = "Update doctor with invalid birth date")]
        [MemberData(nameof(InvalidDates))]
        public void Doctor_UpdateDoctor_InvalidBirthDate(DateTime birthDate)
        {
            //Arrange
            var doctor = GenerateValidDoctor();

            //Act
            var result = Assert.Throws<DomainException>(() => doctor.Update(doctor.Name, doctor.Cpf.Value,
                doctor.Gender, doctor.Phone.Value, birthDate, doctor.Crm));

            //Assert
            Assert.Equal("Data de Nascimento inválida", result.Message);
        }

        [Trait("Doctor", "Invalid")]
        [Theory(DisplayName = "Create doctor with required crm")]
        [InlineData("")]
        [InlineData("      ")]
        [InlineData(null)]
        public void Doctor_UpdateDoctor_RequiredCrm(string crm)
        {
            //Arrange
            var doctor = GenerateValidDoctor();

            //Act
            var result = Assert.Throws<DomainException>(() => doctor.Update(doctor.Name, doctor.Cpf.Value,
                doctor.Gender, doctor.Phone.Value, doctor.BirthDate, crm));

            //Assert
            Assert.Equal("O crm é obrigatório", result.Message);
        }


        [Trait("Doctor", "Invalid")]
        [Fact(DisplayName = "Update doctor with max length crm")]
        public void Doctor_UpdateDoctor_MaxLengthCrm()
        {
            //Arrange
            var doctor = GenerateValidDoctor();
            var crm = new string('c', new Random().Next(11, 20));

            //Act
            var result = Assert.Throws<DomainException>(() => doctor.Update(doctor.Name, doctor.Cpf.Value,
                doctor.Gender, doctor.Phone.Value, doctor.BirthDate, crm));

            //Assert
            Assert.Equal("O crm deve ter no máximo 10 caracteres", result.Message);
        }

        [Trait("Doctor", "Invalid")]
        [Theory(DisplayName = "Update doctor with invalid image path")]
        [MemberData(nameof(InvalidImagePaths))]
        public void Doctor_UpdateDoctor_InvalidImagePath(string filePath)
        {
            //Arrange
            var doctor = GenerateValidDoctor();

            //Act
            var result = Assert.Throws<DomainException>(() => doctor.AddImagePath(filePath));

            //Assert
            Assert.Equal("Imagem inválida", result.Message);
        }

        [Trait("Doctor", "Valid")]
        [Theory(DisplayName = "Update doctor with invalid image path")]
        [MemberData(nameof(ValidImagePaths))]
        public void Doctor_UpdateDoctor_ValidImagePath(string filePath)
        {
            //Arrange
            var doctor = GenerateValidDoctor();

            //Act
            var result = Record.Exception(() => doctor.AddImagePath(filePath));

            //Assert
            Assert.Null(result);
        }

        [Trait("Doctor", "Invalid")]
        [Fact(DisplayName = "Disable doctor")]
        public void Doctor_DisableDoctor_Valid()
        {
            //Arrange
            var doctor = GenerateValidDoctor();

            //Act
            var result = Record.Exception(() => doctor.Disable());

            //Assert
            Assert.Null(result);
            Assert.False(doctor.IsActive);
        }

        [Trait("Doctor", "Invalid")]
        [Fact(DisplayName = "Enable doctor")]
        public void Doctor_EnableDoctor_Valid()
        {
            //Arrange
            var doctor = GenerateValidDoctor();

            //Act
            var result = Record.Exception(() => doctor.Enable());

            //Assert
            Assert.Null(result);
            Assert.True(doctor.IsActive);
        }

        [Trait("Doctor", "Invalid")]
        [Fact(DisplayName = "Delete doctor")]
        public void Doctor_DeleteDoctor_Valid()
        {
            //Arrange
            var doctor = GenerateValidDoctor();

            //Act
            var result = Record.Exception(() => doctor.Delete());

            //Assert
            Assert.Null(result);
            Assert.True(doctor.IsDeleted);
        }

        [Trait("Doctor", "Invalid")]
        [Fact(DisplayName = "Remove image path from doctor")]
        public void Doctor_UpdateDoctor_ValidRemoveImagePath()
        {
            //Arrange
            var doctor = GenerateValidDoctor();

            //Act
            var result = Record.Exception(() => doctor.RemoveImagePath());

            //Assert
            Assert.Null(result);
            Assert.Null(doctor.ImagePath);
        }
    }
}
