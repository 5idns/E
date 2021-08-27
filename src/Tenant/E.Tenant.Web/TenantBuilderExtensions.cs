using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace E.Tenant.Web
{
    /// <summary>
    /// Configure tenant services
    /// </summary>
    public static class TenantBuilderExtensions
    {
        /// <summary>
        /// Register the tenant resolver implementation
        /// </summary>
        /// <typeparam name="TTenantContext"></typeparam>
        /// <param name="tenantBuilder"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static TenantBuilder<TTenantContext> WithHttpAccessor<TTenantContext>(
            this TenantBuilder<TTenantContext> tenantBuilder,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TTenantContext : ITenantContext, new()
        {
            tenantBuilder.WithAccessor<HttpTenantAccessor<TTenantContext>>();
            return tenantBuilder;
        }


        /// <summary>
        /// Register the tenant resolver implementation
        /// </summary>
        /// <typeparam name="TTenantContext"></typeparam>
        /// <param name="tenantBuilder"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static TenantBuilder<TTenantContext> WithHostResolutionStrategy<TTenantContext>(
            this TenantBuilder<TTenantContext> tenantBuilder,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TTenantContext : ITenantContext, new()
        {
            tenantBuilder.ServiceCollection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            tenantBuilder.ServiceCollection.Add(ServiceDescriptor.Describe(typeof(ITenantResolutionStrategy),
                typeof(HostResolutionStrategy), lifetime));
            return tenantBuilder;
        }

        /// <summary>
        /// Register the tenant resolver implementation
        /// </summary>
        /// <typeparam name="TTenantContext"></typeparam>
        /// <param name="tenantBuilder"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static TenantBuilder<TTenantContext> WithRouteResolutionStrategy<TTenantContext>(
            this TenantBuilder<TTenantContext> tenantBuilder,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TTenantContext : ITenantContext, new()
        {
            tenantBuilder.ServiceCollection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            tenantBuilder.ServiceCollection.Add(ServiceDescriptor.Describe(typeof(ITenantResolutionStrategy),
                typeof(RouteResolutionStrategy), lifetime));
            return tenantBuilder;
        }

        /// <summary>
        /// Register the tenant resolver implementation
        /// </summary>
        /// <typeparam name="TTenantContext"></typeparam>
        /// <typeparam name="TStrategy"></typeparam>
        /// <param name="tenantBuilder"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static TenantBuilder<TTenantContext> WithResolutionStrategy<TTenantContext, TStrategy>(this TenantBuilder<TTenantContext> tenantBuilder,
            ServiceLifetime lifetime = ServiceLifetime.Transient)
            where TTenantContext : ITenantContext,new()
            where TStrategy : class, ITenantResolutionStrategy
        {
            tenantBuilder.ServiceCollection.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            tenantBuilder.ServiceCollection.Add(ServiceDescriptor.Describe(typeof(ITenantResolutionStrategy), typeof(TStrategy), lifetime));
            return tenantBuilder;
        }
    }
}
