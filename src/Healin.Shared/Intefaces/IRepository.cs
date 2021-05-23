using Healin.Shared.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Shared.Intefaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        public IUnitOfWork UnitOfWork { get; }

        Task AddAsync(TEntity entity);
        Task AddAsync(IEnumerable<TEntity> entities);
        Task UpdateAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> GetByIdAsync(Guid id, params string[] joins);
        Task<bool> ExistsAsync(Guid id);
    }
}
