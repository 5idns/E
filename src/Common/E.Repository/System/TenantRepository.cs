using System;
using System.Collections.Generic;
using System.Text;

namespace E.Repository.System
{
    public class TenantRepository : PageTableRepository<SystemDbContext, Models.Tenant>
    {
        public TenantRepository(SystemDbContext context) : base(context)
        {
        }
    }
}
