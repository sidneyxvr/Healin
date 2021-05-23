using Healin.Domain.ValueObjects;
using Healin.Shared.Exceptions;
using System;
using Xunit;

namespace Healin.Domain.Test
{
    public class FilePathTest
    {
        public static readonly object[][] ValidImagePaths =
        {
            new object[] { $"{Guid.NewGuid()}.jpeg" },
            new object[] { $"{Guid.NewGuid()}.jpg" },
            new object[] { $"{Guid.NewGuid()}.PNG" },
        };

        public static readonly object[][] InvalidImagePaths =
        {
            new object[] { $"{Guid.NewGuid()}.mp3" },
            new object[] { $"{Guid.NewGuid()}.mp4" },
            new object[] { $"{Guid.NewGuid()}.pdf" },
            new object[] { $"{Guid.NewGuid()}.docx" },
            new object[] { $"{Guid.NewGuid()}.xls" },
        };

        public static readonly object[][] InvalidPDFPaths =
       {
            new object[] { $"{Guid.NewGuid()}.mp3" },
            new object[] { $"{Guid.NewGuid()}.mp4" },
            new object[] { $"{Guid.NewGuid()}.docx" },
            new object[] { $"{Guid.NewGuid()}.xls" },
        };

        [Trait("FilePath", "Valid")]
        [Theory(DisplayName = "Create file path with valid path image")]
        [MemberData(nameof(ValidImagePaths))]
        public void FilePath_IsImage_ShouldBeValid(string path)
        {
            //Arrange
            var filePath = FilePath.CreateImage(path);

            //Act
            var result = filePath.IsImage();

            //Assert
            Assert.True(result);
        }

        [Trait("FilePath", "Invalid")]
        [Theory(DisplayName = "Create file path with invalid path image")]
        [MemberData(nameof(InvalidImagePaths))]
        public void FilePath_IsImage_ShouldBeInvalid(string path)
        {
            //Arrange && Act
            var result = Assert.Throws<DomainException>(() => FilePath.CreateImage(path));

            //Assert
            Assert.Equal("Imagem inválida", result.Message);
        }

        [Trait("FilePath", "Valid")]
        [Fact(DisplayName = "Create file path with valid path pdf")]
        public void FilePath_IsPDF_ShouldBeValid()
        {
            //Arrange
            var path = $"{Guid.NewGuid()}.pdf";
            var filePath = FilePath.CreatePDF(path);

            //Act
            var result = filePath.IsPDF();

            //Assert
            Assert.True(result);
        }

        [Trait("FilePath", "Invalid")]
        [Theory(DisplayName = "Create file path with invalid path pdf")]
        [MemberData(nameof(InvalidPDFPaths))]
        public void FilePath_IsPDF_ShouldBeInvalid(string path)
        {
            //Arrange && Act 
            var result = Assert.Throws<DomainException>(() => FilePath.CreatePDF(path));

            //Assert
            Assert.Equal("PDF inválido", result.Message);
        }
    }
}
