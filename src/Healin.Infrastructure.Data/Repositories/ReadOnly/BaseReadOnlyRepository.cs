using Healin.Infrastructure.Data.Context;
using Healin.Shared.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Healin.Infrastructure.Data.Repositories.ReadOnly
{
    public class BaseReadOnlyRepository<TEntity> where TEntity : Entity
    {
        protected readonly HealinDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public BaseReadOnlyRepository(HealinDbContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        protected virtual IQueryable<TEntity> Page(IQueryable<TEntity> query, int page, int pageSize)
        {
            if (page == 0)
            {
                return query.Take(1000);
            }
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        protected virtual IQueryable<TEntity> FilterOut(IQueryable<TEntity> query, string filter)
        {
            throw new NotImplementedException();
        }

        protected virtual IQueryable<TEntity> Order(IQueryable<TEntity> query, string order)
        {
            throw new NotImplementedException();
        }

        protected virtual IQueryable<TEntity> Search(IQueryable<TEntity> query, string search)
        {
            throw new NotImplementedException();
        }
    }
}
