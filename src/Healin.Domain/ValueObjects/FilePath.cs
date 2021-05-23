using Healin.Shared.Utils;
using System;
using System.Linq;

namespace Healin.Domain.ValueObjects
{
    public class FilePath
    {
        public string Path { get; private set; }
        public string Extension { get => System.IO.Path.GetExtension(Path); }

        private static string[] ValidImageExtensions => new string[] { ".jpeg", ".jpg", ".png" };
        public bool IsImage() 
            => ValidImageExtensions.Any(i => i.Equals(Extension, StringComparison.OrdinalIgnoreCase));
        public static bool IsImage(string filePath) 
            => ValidImageExtensions.Any(i => i.Equals(System.IO.Path.GetExtension(filePath), StringComparison.OrdinalIgnoreCase));

        public bool IsPDF() 
            => Extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase);
        public static bool IsPDF(string filePath) 
            => System.IO.Path.GetExtension(filePath).Equals(".pdf", StringComparison.OrdinalIgnoreCase);

        protected FilePath() { }

        private FilePath(string filePath)
        {
            Path = filePath;
        }

        public static FilePath CreateImage(string filePath)
        {
            AssertionConcern.AssertArgumentTrue(IsImage(filePath), "Imagem inválida");

            return new FilePath(filePath);
        }

        public static FilePath CreatePDF(string filePath)
        {
            AssertionConcern.AssertArgumentTrue(IsPDF(filePath), "PDF inválido");

            return new FilePath(filePath);
        }

        public override string ToString()
        {
            return Path;
        }
    }
}
