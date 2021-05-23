using Healin.Infrastructure.Data.Context;
using Healin.Shared.Data;
using Healin.Shared.Intefaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Healin.Infrastructure.Data.Repositories
{
    public abstract class BaseRespository<T> : IRepository<T> where T : Entity
    {
        protected readonly HealinDbContext _context;
        protected readonly DbSet<T> _dbSet;

        protected BaseRespository(HealinDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        public IUnitOfWork UnitOfWork => _context;

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Dispose()
        {
            _context?.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _dbSet.AnyAsync(entity => entity.Id == id);
        }

        public virtual async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual Task<T> GetByIdAsync(Guid id, params string[] joins)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return Task.CompletedTask;
        }
    }
}
