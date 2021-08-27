using Microsoft.Extensions.DependencyInjection;

namespace E.Tenant
{
    /// <summary>
    /// Nice method to create the tenant builder
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add the services (application specific tenant class)
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static TenantBuilder<T> AddMultiTenant<T>(this IServiceCollection services)
            where T : class, ITenantContext,new()
        {
            return new TenantBuilder<T>(services);
        }

        /// <summary>
        /// Add the services (default tenant class)
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static TenantBuilder<TenantContext> AddMultiTenant(this IServiceCollection services)
            => AddMultiTenant<TenantContext>(services);
    }
}
