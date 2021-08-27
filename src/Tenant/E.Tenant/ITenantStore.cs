using System.Threading.Tasks;

namespace E.Tenant
{
    public interface ITenantStore<T> where T : ITenantContext
    {
        Task<T> GetTenantAsync(string identity);
    }
}
