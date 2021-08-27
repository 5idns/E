using System.Threading.Tasks;

namespace E.Tenant.Store
{
    /// <summary>
    /// In memory store for testing
    /// </summary>
    public class InMemoryTenantStore<T> : ITenantStore<T> where T:ITenantContext
    {
        /// <summary>
        /// Get a tenant for a given identifier
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public async Task<T> GetTenantAsync(string identity)
        {
            var tenant = Tenant<T>.Get(identity);

            return await Task.FromResult(tenant);
        }
    }
}
