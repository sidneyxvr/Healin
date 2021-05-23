using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Healin.Infrastructure.Storage
{
    public class FileStorage : IFileStorage
    {
        private const string FileFolder = "arquivos";
        private const string ImageFolder = "imagens";

        public Task<StorageResult<object>> RemoveFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return Task.FromResult(new StorageResult<object>
                {
                    Succeeded = false,
                    Errors = new List<string> { "arquivo não encontrado" }
                });
            }

            if (File.Exists($"{Directory.GetCurrentDirectory()}/wwwroot/{filePath.Replace("/", @"\")}"))
            {
                File.Delete($"{Directory.GetCurrentDirectory()}/wwwroot/{filePath.Replace("/", @"\")}");
            }

            return Task.FromResult(new StorageResult<object>
            {
                Succeeded = true
            });
        }

        public async Task<StorageResult<string>> SaveFile(IFormFile file)
        {
            if (file.Length == 0)
            {
                return new StorageResult<string>
                {
                    Succeeded = false,
                    Errors = new List<string> { "Arquivo inválido" }
                };
            }


            var pathFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ImageFolder);

            if (!Directory.Exists(pathFolder))
            {
                Directory.CreateDirectory(pathFolder);
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var path = Path.Combine(FileFolder, fileName);

            await using var stream = new FileStream(Path.Combine("wwwroot", path), FileMode.Create);
            await file.CopyToAsync(stream);

            return new StorageResult<string>
            {
                Succeeded = true,
                Result = path.Replace(@"\", "/")
            };
        }

        public async Task<StorageResult<string>> SaveImage(IFormFile file)
        {
            if (file.Length == 0)
            {
                return new StorageResult<string>
                {
                    Succeeded = false,
                    Errors = new List<string> { "Arquivo inválido" }
                };
            }

            var pathFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", ImageFolder);

            if (!Directory.Exists(pathFolder))
            {
                Directory.CreateDirectory(pathFolder);
            }
            var extenstion = Path.GetExtension(file.FileName);
            var fileName = $"{Guid.NewGuid()}{(string.IsNullOrWhiteSpace(extenstion) ? ".png": extenstion)}";
            var path = Path.Combine(ImageFolder, fileName);

            await using var stream = new FileStream(Path.Combine("wwwroot", path), FileMode.Create);
            await file.CopyToAsync(stream);

            return new StorageResult<string>
            {
                Succeeded = true,
                Result = path.Replace(@"\", "/")
            };
        }
    }
}
