using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using E.Common.Entities;
using E.Common.Enumeration;
using E.Exceptions;

namespace E.Repository
{
    public abstract class PageTableRepository<TDbContext, TEntity> : CommonRepository<TDbContext, TEntity>
        where TDbContext : FreeSqlDbContext
        where TEntity : class
    {
        protected PageTableRepository(TDbContext context) : base(context)
        {
        }

        public TableData<TEntity> GetPage<TMember>(
            Expression<Func<TEntity, bool>> exp,
            Expression<Func<TEntity, TMember>> column,
            bool descending = false,
            int start = 0,
            int length = 10
        )
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

                var select = Where(exp);
                if (column != null)
                {
                    if (descending)
                    {
                        select = select.OrderByDescending(column);
                    }
                    else
                    {
                        select = select.OrderBy(column);
                    }
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

        public async Task<TableData<TEntity>> GetPageAsync<TMember>(
            Expression<Func<TEntity, bool>> exp,
            Expression<Func<TEntity, TMember>> column,
            bool descending = false,
            int start = 0,
            int length = 10
        )
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

                var select = Where(exp);
                if (column != null)
                {
                    if (descending)
                    {
                        select = select.OrderByDescending(column);
                    }
                    else
                    {
                        select = select.OrderBy(column);
                    }
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
}
