using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using E.Common.Entities;
using E.Repository;
using Serialize.Linq.Nodes;

namespace E.AutoTable
{
    public class AutoTable
    {
        internal readonly FreeSqlDbContext Context;
        internal readonly AutoTableRepository PageTableRepository;

        public AutoTable(FreeSqlDbContext context)
        {
            Context = context;
            PageTableRepository = new AutoTableRepository(context);
        }

        public async Task<TableData<TEntity>> PagesAsync<TEntity, TMember>(
            ExpressionNode expression,
            ExpressionNode column,
            bool descending = false,
            int page = 0,
            int length = 10)
            where TEntity : class
        {
            var whereExpression = expression.ToBooleanExpression<TEntity>();
            var orderExpression = column.ToExpression<Func<TEntity, TMember>>();
            var data = await PageTableRepository.GetPageAsync(whereExpression, orderExpression, descending,
                start: page * length,
                length: length);
            return data;
        }

        public async Task<TableData<TEntity>> PagesAsync<TEntity, TMember>(
            Expression<Func<TEntity, bool>> exp,
            Expression<Func<TEntity, TMember>> column,
            bool descending = false,
            int page = 0,
            int length = 10)
            where TEntity : class
        {
            var data = await PageTableRepository.GetPageAsync(exp, column, descending, start: page * length,
                length: length);
            return data;
        }

        public async Task<TableData<TEntity>> PagesWithSqlAsync<TEntity, TMember>(
            string sql,
            int page = 0,
            int length = 10)
            where TEntity : class
        {
            var start = page * length;
            if (start < 0)
            {
                start = 0;
            }

            if (length < 1)
            {
                length = 10;
            }

            var select = Context.DbContext.Select<TEntity>().WithSql(sql);

            var dataList = await select.Skip(start).Take(length).ToListAsync<TEntity>();
            var count = await select.CountAsync();
            var data = new TableData<TEntity>(dataList.ToArray(), start, length, count);
            return data;
        }
    }


    public class AutoTable<TEntity>: AutoTable where TEntity : class
    {

        public AutoTable(FreeSqlDbContext context):base(context)
        {
        }

        public async Task<TableData<TEntity>> PagesAsync<TMember>(
            ExpressionNode expression,
            ExpressionNode column,
            bool descending = false,
            int page = 0,
            int length = 10)
        {
            return await PagesAsync<TEntity, TMember>(expression, column, descending, page, length);
        }

        public async Task<TableData<TEntity>> PagesAsync<TMember>(
            Expression<Func<TEntity, bool>> exp,
            Expression<Func<TEntity, TMember>> column,
            bool descending = false,
            int page = 0,
            int length = 10)
        {
            return await PagesAsync<TEntity, TMember>(exp, column, descending, page, length);
        }

        public async Task<TableData<TEntity>> PagesWithSqlAsync<TMember>(
            Expression<Func<TEntity, bool>> exp,
            Expression<Func<TEntity, TMember>> column,
            bool descending = false,
            int page = 0,
            int length = 10)
        {
            return await PagesWithSqlAsync<TEntity, TMember>(exp, column, descending, page, length);
        }
    }
}
