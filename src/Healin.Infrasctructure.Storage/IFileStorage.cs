using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Healin.Infrastructure.Storage
{
    public interface IFileStorage
    {
        Task<StorageResult<string>> SaveFile(IFormFile file);
        Task<StorageResult<object>> RemoveFile(string filePath);
        Task<StorageResult<string>> SaveImage(IFormFile file);
    }
}
