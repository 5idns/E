using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using E.Business.System;
using E.Common.Entities;
using E.Common.Enumeration;
using E.Exceptions;
using E.Tenant;

namespace E.OAuth.Web.Controllers
{
    public class TenantController : Controller
    {
        private readonly TenantBusiness _tenantBusiness;
        private readonly ITenantAccessor _tenantAccessor;

        public TenantController(TenantBusiness tenantBusiness, ITenantAccessor tenantAccessor)
        {
            _tenantBusiness = tenantBusiness;
            _tenantAccessor = tenantAccessor;
        }

        public async Task<TableData<Repository.System.Models.Tenant>> Tenants(int page=0,int length=10)
        {
            var  data = await _tenantBusiness.GetPageAsync(f => true, f => f.CreateTime, start: page * length, length: length);
            return data;
        }

        public async Task<CommonResult> InsertOrUpdateAsync([FromForm] Repository.System.Models.Tenant tenant)
        {
            try
            {
                await _tenantBusiness.InsertOrUpdateAsync(tenant);
                return new CommonResult(Status.Success, "修改租户信息成功");
            }
            catch (SysException e)
            {
                return new CommonResult(e.Status, e.Message);
            }
            catch (Exception e)
            {
                return new CommonResult(Status.DatabaseError, $"修改租户信息失败{e.Message}");
            }
            
        }

        public Task<ITenantContext> Info()
        {
            var tenant = _tenantAccessor.Context;
            return Task.FromResult(tenant);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
