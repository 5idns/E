using System.Threading.Tasks;

namespace E.Tenant
{
    /// <summary>
    /// Tenant access service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TenantAccessService<T> where T : ITenantContext
    {
        private readonly ITenantResolutionStrategy _tenantResolutionStrategy;
        private readonly ITenantStore<T> _tenantStore;

        public TenantAccessService(ITenantResolutionStrategy tenantResolutionStrategy, ITenantStore<T> tenantStore)
        {
            _tenantResolutionStrategy = tenantResolutionStrategy;
            _tenantStore = tenantStore;
        }

        /// <summary>
        /// Get the current tenant
        /// </summary>
        /// <returns></returns>
        public async Task<T> GetTenantAsync()
        {
            var tenantIdentifier = await _tenantResolutionStrategy.GetTenantIdentityAsync();
            var context = Tenant<T>.Get(tenantIdentifier);
            if (context != null)
            {
                return context;
            }
            context = await _tenantStore.GetTenantAsync(tenantIdentifier);
            if (context != null)
            {
                Tenant<T>.ChangeTenant(context.Identity.ToString("D"), context);
            }

            return context;
        }
    }
}
