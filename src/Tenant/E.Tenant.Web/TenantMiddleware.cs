using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace E.Tenant.Web
{
    internal class TenantMiddleware<T> where T : ITenantContext
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Items.ContainsKey(Constants.HttpContextTenantKey))
            {
                if (context.RequestServices.GetService(typeof(TenantAccessService<T>)) is TenantAccessService<T> tenantService)
                {
                    context.Items.Add(Constants.HttpContextTenantKey, await tenantService.GetTenantAsync());
                }
            }

            //Continue processing
            if (_next != null)
                await _next(context);
        }
    }
}
