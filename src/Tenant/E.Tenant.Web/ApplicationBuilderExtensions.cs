using Microsoft.AspNetCore.Builder;

namespace E.Tenant.Web
{
    /// <summary>
    /// Nice method to register our middleware
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Use the Tenant Middleware to process the request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultiTenant<T>(this IApplicationBuilder builder)
            where T : class, ITenantContext
            => builder.UseMiddleware<TenantMiddleware<T>>();


        /// <summary>
        /// Use the Tenant Middleware to process the request
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMultiTenant(this IApplicationBuilder builder)
            => builder.UseMiddleware<TenantMiddleware<TenantContext>>();
    }
}
