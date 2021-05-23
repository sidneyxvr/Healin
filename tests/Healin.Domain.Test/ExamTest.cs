using Bogus;
using Healin.Domain.Models;
using Healin.Shared.Exceptions;
using System;
using Xunit;

namespace Healin.Domain.Test
{
    public class ExamTest
    {
        private static Exam GenerateExam()
        {
            var examFake = new Faker<Exam>("pt_BR")
                .CustomInstantiator(f => Exam.New(f.Random.Utf16String(1, 50)));

            return examFake;
        }

        [Trait("Exam", "Valid")]
        [Fact]
        public void Exam_NewExam_ShouldBeValid()
        {
            //Arrange && Act
            var result = Record.Exception(() => GenerateExam());

            //Assert
            Assert.Null(result);
        }

        [Trait("Exam", "Valid")]
        [Fact]
        public void Exam_Update_ShouldBeValid()
        {
            //Arrange
            var exam = GenerateExam();
            var name = new string('n', new Random().Next(0, 50));

            //Act
            var result = Record.Exception(() => exam.Update(name));

            //Assert
            Assert.Null(result);
        }

        [Trait("Exam", "Valid")]
        [Fact]
        public void Exam_Delete_ShouldBeValid()
        {
            //Arrange
            var exam = GenerateExam();
            
            //Act
            var result = Record.Exception(() => exam.Delete());

            //Assert
            Assert.Null(result);
        }

        [Trait("Exam", "Valid")]
        [Fact]
        public void Exam_Enable_ShouldBeValid()
        {
            //Arrange
            var exam = GenerateExam();

            //Act
            var result = Record.Exception(() => exam.Enable());

            //Assert
            Assert.Null(result);
            Assert.True(exam.IsActive);
        }

        [Trait("Exam", "Valid")]
        [Fact]
        public void Exam_Disable_ShouldBeValid()
        {
            //Arrange
            var exam = GenerateExam();

            //Act
            var result = Record.Exception(() => exam.Disable());

            //Assert
            Assert.Null(result);
            Assert.False(exam.IsActive);
        }

        [Trait("Exam", "Inalid")]
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Exam_NewExam_EmptyName(string name)
        {
            //Act
            var result = Assert.Throws<DomainException>(() => Exam.New(name));

            //Assert
            Assert.Equal("O nome é obrigatório", result.Message);
        }

        [Trait("Exam", "Inalid")]
        [Fact]
        public void Exam_NewExam_MaxLengthName()
        {
            //Arrange
            var name = new string('n', new Random().Next(51, 100));

            //Act
            var result = Assert.Throws<DomainException>(() => Exam.New(name));

            //Assert
            Assert.Equal("O nome deve ter no máximo 50 caracteres", result.Message);
        }

        [Trait("Exam", "Inalid")]
        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Exam_Update_EmptyName(string name)
        {
            //Arrange
            var exam = GenerateExam();
            var isActive = new Random().Next(0, 1) == 1;

            //Act
            var result = Assert.Throws<DomainException>(() => exam.Update(name));

            //Assert
            Assert.Equal("O nome é obrigatório", result.Message);
        }

        [Trait("Exam", "Inalid")]
        [Fact]
        public void Exam_Update_MaxLengthName()
        {
            //Arrange
            var exam = GenerateExam();
            var name = new string('n', new Random().Next(51, 100));
            var isActive = new Random().Next(0, 1) == 1;

            //Act
            var result = Assert.Throws<DomainException>(() => exam.Update(name));

            //Assert
            Assert.Equal("O nome deve ter no máximo 50 caracteres", result.Message);
        }
    }
}
