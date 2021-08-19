using System;
using System.Linq.Expressions;
using FreeSql;

namespace E.Repository
{
    public abstract class CommonRepository<TDbContext,TEntity> : BaseRepository<TEntity>
        where TDbContext:FreeSqlDbContext
        where TEntity : class
    {
        protected CommonRepository(TDbContext context, Expression<Func<TEntity, bool>> filter = null, Func<string, string> asTable = null)
            : base(context.DbContext, filter, asTable)
        {
        }
    }

    public abstract class CommonRepository<TDbContext, TEntity, TKey> : BaseRepository<TEntity, TKey> 
        where TDbContext : FreeSqlDbContext
        where TEntity : class
    {
        protected CommonRepository(TDbContext context, Expression<Func<TEntity, bool>> filter = null, Func<string, string> asTable = null)
            : base(context.DbContext, filter, asTable)
        {
        }
    }
}
