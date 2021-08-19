using System;
using System.Threading.Tasks;
using E.Enumeration;
using E.Exceptions;

namespace E.Repository.System
{
    public class StatusInfoRepository : PageTableRepository<SystemDbContext, Models.StatusInfo>
    {
        public StatusInfoRepository(SystemDbContext context) : base(context)
        {
        }

        public Models.StatusInfo[] GetAll()
        {
            try
            {
                var dbConfigs = Where(f => true).ToList();
                return dbConfigs.ToArray();
            }
            catch (Exception e)
            {
                throw new SysException(Status.DatabaseError, "加载状态信息时发生错误", e);
            }
        }

        public async Task<Models.StatusInfo[]> GetAllAsync()
        {
            try
            {
                var dbConfigs = await Where(f => true).ToListAsync();
                return dbConfigs.ToArray();
            }
            catch (Exception e)
            {
                throw new SysException(Status.DatabaseError, "加载状态信息时发生错误", e);
            }
        }

        public Models.StatusInfo[] GetAll(long status)
        {
            try
            {
                var dbConfigs = Where(f => f.Status == status).ToList();
                return dbConfigs.ToArray();
            }
            catch (Exception e)
            {
                throw new SysException(Status.DatabaseError, "加载状态信息时发生错误", e);
            }
        }

        public async Task<Models.StatusInfo[]> GetAllAsync(long status)
        {
            try
            {
                var dbConfigs = await Where(f => f.Status == status).ToListAsync();
                return dbConfigs.ToArray();
            }
            catch (Exception e)
            {
                throw new SysException(Status.DatabaseError, "加载状态信息时发生错误", e);
            }
        }

        public async Task<Models.StatusInfo> Add(Models.StatusInfo statusInfo)
        {
            if (statusInfo == null)
            {
                throw new SysException(Status.ParameterError, "状态信息不能为空");
            }
            if (string.IsNullOrWhiteSpace(statusInfo.Culture))
            {
                throw new SysException(Status.ParameterError, "区域信息不能为空");
            }
            if (string.IsNullOrWhiteSpace(statusInfo.Description))
            {
                throw new SysException(Status.ParameterError, "描述不能为空");
            }
            try
            {
                var userIdCount = await Where(f => f.Status == statusInfo.Status && f.Culture == statusInfo.Culture).CountAsync();
                if (userIdCount <= 0)
                {
                    statusInfo = await InsertAsync(statusInfo);
                    return statusInfo;
                }
                else
                {
                    throw new SysException(Status.NameExist, "状态信息已经存在");
                }
            }
            catch (Exception e)
            {
                throw new SysException(Status.DatabaseError, "数据库运行错误", e);
            }
        }
    }
}
