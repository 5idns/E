using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using E.Common.Entities;
using E.Repository.System;
using E.Repository.System.Models;

namespace E.Business.System
{
    public class TenantBusiness
    {
        private readonly TenantRepository _tenantRepository;

        public TenantBusiness(TenantRepository tenantRepository)
        {
            _tenantRepository = tenantRepository;
        }

        public async Task<Tenant> Get(Guid identity)
        {
            var tenant = await _tenantRepository.Where(f => f.Identity == identity).FirstAsync();
            return tenant;
        }

        public async Task<Tenant> InsertOrUpdateAsync(Tenant tenant)
        {
            if (tenant.Identity == Guid.Empty)
            {
                tenant.Identity = Guid.NewGuid();
            }

            tenant = await _tenantRepository.InsertOrUpdateAsync(tenant);
            return tenant;
        }

        public async Task<TableData<Tenant>> GetPageAsync<TMember>(
            Expression<Func<Tenant, bool>> exp,
            Expression<Func<Tenant, TMember>> column,
            bool descending = false,
            int start = 0,
            int length = 10
        )
        {
            var result = await _tenantRepository.GetPageAsync(exp, column, descending, start, length);
            return result;
        }
    }
}
