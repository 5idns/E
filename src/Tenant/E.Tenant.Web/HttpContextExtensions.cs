using Microsoft.AspNetCore.Http;

namespace E.Tenant.Web
{
    /// <summary>
    /// Extensions to HttpContext to make multi-tenant easier to use
    /// </summary>
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Returns the current tenant
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        /// <returns></returns>
        public static T GetTenant<T>(this HttpContext context) where T : ITenantContext
        {
            if (!context.Items.ContainsKey(Constants.HttpContextTenantKey))
                return default;
            var tenant = context.Items[Constants.HttpContextTenantKey];
            if (tenant is T tenantContext)
            {
                return tenantContext;
            }
            return default;
        }

        /// <summary>
        /// Returns the current Tenant
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ITenantContext GetTenant(this HttpContext context)
        {
            return context.GetTenant<ITenantContext>();
        }
    }
}
