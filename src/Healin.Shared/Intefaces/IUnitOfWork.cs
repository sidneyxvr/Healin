using System.Threading.Tasks;

namespace Healin.Shared.Intefaces
{
    public interface IUnitOfWork
    {
        Task<bool> CommitAsync();
    }
}
