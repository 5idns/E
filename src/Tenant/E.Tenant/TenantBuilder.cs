using E.Common.Collections;
using E.Tenant.Store;
using Microsoft.Extensions.DependencyInjection;

namespace E.Tenant
{
    /// <summary>
    /// Configure tenant serviceCollection
    /// </summary>
    public class TenantBuilder<T> where T : ITenantContext, new()
    {
        public IServiceCollection ServiceCollection { get; }

        public TenantBuilder(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<TenantAccessService<T>>();
            ServiceCollection = serviceCollection;
        }

        public TenantBuilder<T> WithAccessor<TAccessor>(ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TAccessor : class, ITenantAccessor<T>
        {
            ServiceCollection.AddIfNotExist(ServiceDescriptor.Describe(typeof(ITenantAccessor), typeof(TAccessor), lifetime));
            ServiceCollection.AddIfNotExist(ServiceDescriptor.Describe(typeof(ITenantAccessor<T>), typeof(TAccessor), lifetime));
            return this;
        }
        /*
        /// <summary>
        /// Register the tenant resolver implementation
        /// </summary>
        /// <typeparam name="TStrategy"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public TenantBuilder<T> WithResolutionStrategy<TStrategy>(ServiceLifetime lifetime = ServiceLifetime.Transient) where TStrategy : class, ITenantResolutionStrategy
        {
            ServiceCollection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            ServiceCollection.Add(ServiceDescriptor.Describe(typeof(ITenantResolutionStrategy), typeof(TStrategy), lifetime));
            return this;
        }*/

        /// <summary>
        /// Register the tenant store implementation
        /// </summary>
        /// <typeparam name="TStore"></typeparam>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public TenantBuilder<T> WithStore<TStore>(ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TStore : class, ITenantStore<T>
        {
            ServiceCollection.Add(ServiceDescriptor.Describe(typeof(ITenantStore<T>), typeof(TStore), lifetime));
            return this;
        }

        /// <summary>
        /// Register the tenant store implementation
        /// </summary>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public TenantBuilder<T> WithInMemoryStore(ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            ServiceCollection.Add(ServiceDescriptor.Describe(typeof(ITenantStore<T>), typeof(InMemoryTenantStore<T>),
                lifetime));
            return this;
        }

        /// <summary>
        /// Register the tenant store implementation
        /// </summary>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public TenantBuilder<T> WithDatabaseStore(ServiceLifetime lifetime = ServiceLifetime.Transient)
        {
            ServiceCollection.Add(ServiceDescriptor.Describe(typeof(ITenantStore<T>), typeof(DatabaseTenantStore<T>),
                lifetime));
            return this;
        }
    }
}
