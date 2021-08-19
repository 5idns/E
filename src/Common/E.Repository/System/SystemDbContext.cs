using FreeSql;
using Microsoft.Extensions.Configuration;

namespace E.Repository.System
{
    public partial class SystemDbContext : FreeSqlDbContext
    {
        private readonly IFreeSql _freeSql;
        public SystemDbContext(IConfiguration configuration):base(configuration.GetSection("DbOptions:System"))
        {
        }

        public SystemDbContext(IFreeSql freeSql):base(freeSql)
        {
            _freeSql = freeSql;
        }
    }
}
