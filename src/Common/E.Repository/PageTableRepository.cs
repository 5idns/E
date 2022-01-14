using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using E.Common.Entities;
using E.Common.Enumeration;
using E.Exceptions;

namespace E.Repository
{
    public abstract class PageTableRepository
    {
        private readonly FreeSqlDbContext _context;
        protected PageTableRepository(FreeSqlDbContext context)
        {
            _context = context;
        }

        public TableData<TEntity> GetPage<TEntity, TMember>(
            Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, TMember>> orderColumn,
            bool descending = false,
            int start = 0,
            int length = 10
        )
        where TEntity : class
        {
            try
            {
                if (start < 0)
                {
                    start = 0;
                }

                if (length < 1)
                {
                    length = 10;
                }

                var select = _context.DbContext.Select<TEntity>().Where(whereExpression);
                if (orderColumn != null)
                {
                    select = descending ? select.OrderByDescending(orderColumn) : select.OrderBy(orderColumn);
                }

                var dataList = select.Skip(start).Take(length).ToList();
                var count = select.Count();
                var data = new TableData<TEntity>(dataList.ToArray(), start, length, count);
                return data;
            }
            catch (Exception e)
            {
                throw new SysException(Status.DatabaseError, "加载数据时发生错误", e);
            }
        }

        public async Task<TableData<TEntity>> GetPageAsync<TEntity, TMember>(
            Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, TMember>> orderColumn,
            bool descending = false,
            int start = 0,
            int length = 10
        )
        where TEntity : class
        {
            try
            {
                if (start < 0)
                {
                    start = 0;
                }

                if (length < 1)
                {
                    length = 10;
                }

                var select = _context.DbContext.Select<TEntity>().Where(whereExpression);
                if (orderColumn != null)
                {
                    select = descending ? select.OrderByDescending(orderColumn) : select.OrderBy(orderColumn);
                }

                var dataList = await select.Skip(start).Take(length).ToListAsync();
                var count = await select.CountAsync();
                var data = new TableData<TEntity>(dataList.ToArray(), start, length, count);
                return data;
            }
            catch (Exception e)
            {
                throw new SysException(Status.DatabaseError, "加载数据时发生错误", e);
            }
        }
    }

    public abstract class PageTableRepository<TDbContext>:PageTableRepository
        where TDbContext : FreeSqlDbContext
    {
        private readonly TDbContext _context;
        protected PageTableRepository(TDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
