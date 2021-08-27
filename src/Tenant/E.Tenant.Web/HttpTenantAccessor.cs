using Microsoft.AspNetCore.Http;

namespace E.Tenant.Web
{
    public class HttpTenantAccessor<T> : ITenantAccessor<T> where T : ITenantContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpTenantAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public T TenantContext => _httpContextAccessor.HttpContext.GetTenant<T>();

        public ITenantContext Context => TenantContext;
    }
}
