using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace E.Tenant.Web
{
    public class RouteResolutionStrategy : ITenantResolutionStrategy
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RouteResolutionStrategy(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get the tenant identifier
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetTenantIdentityAsync()
        {
            var routeValuesFeature = _httpContextAccessor.HttpContext.Features[typeof(IRouteValuesFeature)]
                as IRouteValuesFeature;

            var routeValues = routeValuesFeature?.RouteValues;

            //Note: endpoint will be null, if there was no
            //route match found for the request by the endpoint route resolver middleware
            if (routeValues != null)
            {
                if (routeValues.ContainsKey(Constants.RouteTenantKey))
                {
                    return routeValues[Constants.RouteTenantKey].ToString();
                }
            }

            return await Task.FromResult(_httpContextAccessor.HttpContext.Request.Host.Host);
        }
    }
}
