using System;
using System.Threading.Tasks;
using E.Common.Serialization;
using E.Repository.System;

namespace E.Tenant.Store
{
    public class DatabaseTenantStore<T> : ITenantStore<T> where T : ITenantContext, new()
    {
        private readonly TenantRepository _tenantRepository;

        public DatabaseTenantStore(TenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        /// <summary>
        /// Get a tenant for a given identifier
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public async Task<T> GetTenantAsync(string identity)
        {
            Repository.System.Models.Tenant tenant;
            if (Guid.TryParse(identity, out var guidIdentity))
            {
                tenant = await _tenantRepository.Where(f => f.Identity == guidIdentity).FirstAsync();
                if (tenant != null)
                {
                    var tenantContext = new T
                    {
                        TenantId = tenant.Id.ToString(),
                        Name = tenant.Name,
                        Identity = tenant.Identity,
                        HostIdentity = tenant.HostIdentity.Deserialize<string[]>()
                    };
                    
                    return tenantContext;
                }
            }

            tenant = await _tenantRepository.Where(f => f.HostIdentity.Contains($"\"{identity}\"")).FirstAsync();
            if (tenant != null)
            {
                var tenantContext = new T
                {
                    TenantId = tenant.Id.ToString(),
                    Name = tenant.Name,
                    Identity = tenant.Identity,
                    HostIdentity = tenant.HostIdentity.Deserialize<string[]>()
                };
                return tenantContext;
            }

            return default;
        }
    }
}
