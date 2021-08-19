using System.Threading;

namespace E.Common.Tenant
{
    /// <summary>
    /// 租户上下文
    /// </summary>
    public class TenantContext
    {
        /// <summary>
        /// 异步本地租户ID
        /// </summary>
        private static readonly AsyncLocal<string> AsyncLocalTenantId;

        private static readonly AsyncLocal<TenantContext> AsyncLocalTenantContext;

        static TenantContext()
        {
            AsyncLocalTenantId = new AsyncLocal<string>();
            AsyncLocalTenantContext = new AsyncLocal<TenantContext>();
        }

        public static TenantContext ChangeTenant(string tenantId)
        {
            AsyncLocalTenantId.Value = tenantId;
            var tenantContext = AsyncLocalTenantContext.Value ?? (AsyncLocalTenantContext.Value = new TenantContext());
            tenantContext.TenantId = tenantId;
            return tenantContext;
        }

        public static TenantContext Context => AsyncLocalTenantContext.Value ?? (AsyncLocalTenantContext.Value = new TenantContext());

        public string TenantId { get; private set; }
    }
}
