using System;
using System.Collections.Generic;
using System.Reflection;
using E.Repository.Configurations;
using FreeSql;
using FreeSql.DataAnnotations;
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
            DbContext.CodeFirst.SyncStructure(types);
            return this;
        }

        /// <summary>
        /// 同步数据库实体
        /// </summary>
        /// <typeparam name="TEntity">实体类型</typeparam>
        /// <returns></returns>
        public FreeSqlDbContext SyncStructure<TEntity>()
        {
            DbContext.CodeFirst.SyncStructure<TEntity>();
            return this;
        }

        private static Type[] GetTypesByTableAttribute(Assembly[] assemblies)
        {
            List<Type> tableAssembies = new List<Type>();
            foreach (Assembly assembly in assemblies)
            foreach (Type type in assembly.GetExportedTypes())
            foreach (Attribute attribute in type.GetCustomAttributes())
                if (attribute is TableAttribute tableAttribute)
                    if (tableAttribute.DisableSyncStructure == false)
                        tableAssembies.Add(type);

            return tableAssembies.ToArray();
        }

        public FreeSqlDbContext SyncStructure(params Assembly[] assemblies)
        {
            var types = GetTypesByTableAttribute(assemblies);
            DbContext.CodeFirst.SyncStructure(types);
            return this;
        }
    }
}
