using E.Exceptions;
using FreeSql;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Linq;
using System.Reflection;
using E.Common.Enumeration;
using E.Repository.Configurations;
using E.Repository.System;
using E.Repository.System.Models;
using Microsoft.Extensions.Configuration;

namespace E.Repository
{
    /// <summary>
    /// Repository 扩展库
    /// </summary>
    public static class RepositoryExtensions
    {
        public static IServiceCollection RegisterSystemDb(this IServiceCollection services,
            IConfiguration configuration,
            Action<Func<long, ConcurrentDictionary<CultureInfo, string>>> configStatusLazyLoading = null)
        {
            var dbOptions = configuration.GetSection("DbOptions:System").Get<DbOptions>();
            IFreeSql CreateFreeSql()
            {
                var newFreeSql = new FreeSqlBuilder().UseConnectionString(dbOptions.DataType, dbOptions.ConnectionString)
                    .UseAutoSyncStructure(true)
                    .Build();
                newFreeSql.UseJsonMap();
                return newFreeSql;
            }

            var freeSql = CreateFreeSql();
            var systemDbContext = new SystemDbContext(freeSql);
            services.AddSingleton(systemDbContext);
            try
            {
                var dbConfigRepository = new DbConfigRepository(systemDbContext);
                var dbConfigs = dbConfigRepository.GetAll();
                if (dbConfigs != null && dbConfigs.Any())
                {
                    foreach (var dbConfig in dbConfigs)
                    {
                        var tenantFreeSql = new FreeSqlBuilder()
                            .UseConnectionString(dbConfig.DataType, dbConfig.ConnectionString)
                            .UseAutoSyncStructure(true)
                            .Build();
                        tenantFreeSql.UseJsonMap();
                        //IdleBus.Register(dbConfig.Id.ToString(), () => tenantFreeSql);
                    }
                }
                services.RegisterRepository<DbConfigRepository>();

            }
            catch (Exception e)
            {
                throw new SysException(Status.DatabaseError, "连接租户数据库时发生错误", e);
            }

            try
            {
                var statusInfoRepository = new StatusInfoRepository(systemDbContext);
                services.RegisterRepository<StatusInfoRepository>();
                if (configStatusLazyLoading != null)
                {
                    ConcurrentDictionary<CultureInfo, string> Func(long statusCode)
                    {
                        var statusInfos = statusInfoRepository.GetAll(statusCode);
                        var dictionary = new ConcurrentDictionary<CultureInfo, string>();
                        foreach (var statusInfo in statusInfos)
                        {
                            CultureInfo cultureInfo = CultureInfo.GetCultureInfo(statusInfo.Culture);
                            dictionary.TryAdd(cultureInfo, statusInfo.Description);
                        }

                        return dictionary;
                    }

                    configStatusLazyLoading(Func);
                }
            }
            catch (Exception e)
            {
                throw new SysException(Status.DatabaseError, "连接租户数据库时发生错误", e);
            }

            return services;
        }

        /// <summary>
        /// 注册数据库连接
        /// <para>并同时注册主数据库中配置的租户数据库连接</para>
        /// </summary>
        /// <param name="services">依赖注入容器</param>
        /// <param name="configuration"></param>
        /// <param name="section"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterConnection<TDbContext>(this IServiceCollection services,
            IConfiguration configuration,string section = "DbOptions:System")
            where TDbContext : FreeSqlDbContext,new()
        {
            var dbOptions = configuration.GetSection(section).Get<DbOptions>();
            IFreeSql CreateFreeSql()
            {
                var newFreeSql = new FreeSqlBuilder().UseConnectionString(dbOptions.DataType, dbOptions.ConnectionString)
                    .UseAutoSyncStructure(true)
                    .Build();
                newFreeSql.UseJsonMap();
                return newFreeSql;
            }
            var freeSql = CreateFreeSql();
            var dbContext = (TDbContext)Activator.CreateInstance(typeof(TDbContext), freeSql);
            if (dbContext != null)
            {
                services.AddSingleton(dbContext);
            }
            return services;
        }

        public static FreeSqlDbContext RegisterConnection(this DbConfig dbConfig)
        {
            IFreeSql CreateFreeSql()
            {
                var newFreeSql = new FreeSqlBuilder().UseConnectionString(dbConfig.DataType, dbConfig.ConnectionString)
                    .UseAutoSyncStructure(true)
                    .Build();
                newFreeSql.UseJsonMap();
                return newFreeSql;
            }
            
            var freeSql = CreateFreeSql();
            var dbContext = (FreeSqlDbContext)Activator.CreateInstance(dbConfig.DbContextType, freeSql);
            return dbContext;
        }

        public static IServiceCollection RegisterRepository<T>(this IServiceCollection services) where T : class, IBaseRepository
        {

            services.AddScoped<T>();
            return services;
        }

        public static IServiceCollection RegisterAssembliesRepository(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies?.Any() == true)
                foreach (var assembly in assemblies) //批量注册
                    foreach (var repo in assembly.GetTypes().Where(a => a.IsAbstract == false && typeof(IBaseRepository).IsAssignableFrom(a)))
                        services.AddScoped(repo);
            return services;
        }

        //public static IServiceCollection AddIdleBusRepository(this IServiceCollection services, params Assembly[] assemblies)
        //{
        //    services.AddSingleton(IdleBus);
        //    //services.AddScoped(typeof(IBaseRepository<>), typeof(CommonRepository<>));
        //    //services.AddScoped(typeof(BaseRepository<>), typeof(CommonRepository<>));
        //    //services.AddScoped(typeof(IBaseRepository<,>), typeof(CommonRepository<,>));
        //    //services.AddScoped(typeof(BaseRepository<,>), typeof(CommonRepository<,>));
        //    if (assemblies?.Any() == true)
        //        foreach (var assembly in assemblies) //批量注册
        //            foreach (var repo in assembly.GetTypes().Where(a => a.IsAbstract == false && typeof(IBaseRepository).IsAssignableFrom(a)))
        //                services.AddScoped(repo);
        //    return services;
        //}
    }
}
