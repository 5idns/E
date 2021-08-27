using E.Repository.System.Models;
using FreeSql;
using Microsoft.Extensions.Configuration;

namespace E.Repository.System
{
    public partial class SystemDbContext : FreeSqlDbContext
    {
        public SystemDbContext(IConfiguration configuration):base(configuration.GetSection("DbOptions:System"),
            builder =>
            {
                builder.UseAutoSyncStructure(true);
            })
        {
        }

        public SystemDbContext(IFreeSql freeSql):base(freeSql)
        {
        }

        public virtual DbSet<DbConfig> DbConfigs { get; set; }

        public virtual DbSet<StatusInfo> StatusInfos { get; set; }

        public virtual DbSet<Tenant> Tenants { get; set; }
    }
}
