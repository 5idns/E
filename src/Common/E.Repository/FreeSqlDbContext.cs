using System;
using System.Reflection;
using E.Repository.Configurations;
using FreeSql;
using Microsoft.Extensions.Configuration;

namespace E.Repository
{
    public abstract class FreeSqlDbContext
    {
        protected FreeSqlDbContext(IConfiguration configuration)
        {
            var dbOptions = configuration.Get<DbOptions>();

            IFreeSql CreateFreeSql()
            {
                var newFreeSql = new FreeSqlBuilder()
                    .UseConnectionString(dbOptions.DataType, dbOptions.ConnectionString)
                    .UseAutoSyncStructure(true)
                    .Build();
                newFreeSql.UseJsonMap();
                return newFreeSql;
            }

            DbContext = CreateFreeSql();
        }

        protected FreeSqlDbContext(IFreeSql freeSql)
        {
            DbContext = freeSql;
        }

        public IFreeSql DbContext { get; }

        /// <summary>
        /// 同步数据库实体
        /// </summary>
        /// <param name="types">实体类型</param>
        /// <returns></returns>
        public FreeSqlDbContext SyncStructure(params Type[] types)
        {
            DbContext.SyncStructure(types);
            return this;
        }

        /// <summary>
        /// 同步数据库实体
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <returns></returns>
        public FreeSqlDbContext SyncStructure<TEntity>()
        {
            DbContext.SyncStructure<TEntity>();
            return this;
        }

        public FreeSqlDbContext SyncStructure(params Assembly[] assemblies)
        {
            DbContext.SyncStructure(assemblies);
            return this;
        }
    }
}
