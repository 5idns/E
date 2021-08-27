using System.Threading;
using System.Threading.Tasks;
using E.Tenant;
using Microsoft.AspNetCore.Mvc;

namespace E.Bots.Telegram
{
    /// <summary>
    /// Telegram
    /// </summary>
    [Route("{Tenant}/Telegram/[controller]")]
    public class SettingController : ControllerBase
    {
        private readonly ITenantAccessor _tenantAccessor;
        private readonly ConfigureWebhook _configureWebhook;

        public SettingController(ITenantAccessor tenantAccessor, ConfigureWebhook configureWebhook)
        {
            _tenantAccessor = tenantAccessor;
            _configureWebhook = configureWebhook;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var tenant = _tenantAccessor.Context;
            var cancelSource = new CancellationTokenSource();
            await _configureWebhook.SetAsync(tenant, cancelSource.Token);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            var tenant = _tenantAccessor.Context;
            var cancelSource = new CancellationTokenSource();
            await _configureWebhook.DeleteAsync(tenant, cancelSource.Token);
            return Ok();
        }
    }
}
