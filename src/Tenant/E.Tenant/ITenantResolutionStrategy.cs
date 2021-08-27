
using System.Threading.Tasks;

namespace E.Tenant
{
    public interface ITenantResolutionStrategy
    {
        Task<string> GetTenantIdentityAsync();
    }
}
