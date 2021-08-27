using System;
using System.Threading;
using System.Threading.Tasks;
using E.Tenant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Senparc.Weixin.Work;
using Senparc.Weixin.Work.Containers;
using Senparc.Weixin.Work.Entities;

namespace E.Bots.WeiXin
{
    /// <summary>
    /// 企业号对接测试
    /// </summary>
    [Route("{Tenant}/WeiXin[controller]")]
    public class WorkController : Controller
    {
        /*
     重要提示
     
  1. 当前 Controller 展示了有特殊自定义需求的 MessageHandler 处理方案，
     可以高度控制消息处理过程的每一个细节，
     如果仅常规项目使用，可以直接使用中间件方式，参考 startup.cs：
     app.UseMessageHandlerForWork("/WorkAsync", WorkCustomMessageHandler.GenerateMessageHandler, options => ...);
  2. 目前 Senparc.Weixin SDK 已经全面转向异步方法驱动，
     因此建议使用异步方法（如：messageHandler.ExecuteAsync()），不再推荐同步方法。
 */

        private readonly BotConfigurationFactory _configuration;
        private readonly ITenantAccessor _tenantAccessor;
        private readonly ILogger<WorkController> _logger;

        //public static readonly string Token = Config.SenparcWeixinSetting.WorkSetting.WeixinCorpToken;//与企业微信账号后台的Token设置保持一致，区分大小写。
        //public static readonly string EncodingAESKey = Config.SenparcWeixinSetting.WorkSetting.WeixinCorpEncodingAESKey;//与微信企业账号后台的EncodingAESKey设置保持一致，区分大小写。
        //public static readonly string CorpId = Config.SenparcWeixinSetting.WorkSetting.WeixinCorpId;//与微信企业账号后台的CorpId设置保持一致，区分大小写。


        public WorkController(ILogger<WorkController> logger,ITenantAccessor tenantAccessor, BotConfigurationFactory configuration)
        {
            _logger = logger;
            _tenantAccessor = tenantAccessor;
            _configuration = configuration;
        }

        /// <summary>
        /// 微信后台验证地址（使用Get），企业微信后台应用的“修改配置”的Url填写如：http://sdk.weixin.senparc.com/work
        /// </summary>
        [HttpGet]
        public ActionResult Get(string msg_signature = "", string timestamp = "", string nonce = "", string echostr = "")
        {
            var tenant = _tenantAccessor.Context;
            var botConfig = _configuration.Get<WeiXinWorkConfiguration>(tenant.TenantId);
            //return Content(echostr); //返回随机字符串则表示验证通过
            var verifyUrl = Signature.VerifyURL(botConfig.WeixinCorpToken,
                botConfig.WeixinCorpEncodingAESKey,
                botConfig.WeixinCorpId,
                msg_signature,
                timestamp,
                nonce,
                echostr);
            if (verifyUrl != null)
            {
                return Content(verifyUrl); //返回解密后的随机字符串则表示验证通过
            }
            else
            {
                return Content("如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。");
            }
        }

        /// <summary>
        /// 微信后台验证地址（使用Post），企业微信后台应用的“修改配置”的Url填写如：http://sdk.weixin.senparc.com/work
        /// </summary>
        [HttpPost]
        [ActionName("Index")]
        public async Task<ActionResult> Post(PostModel postModel)
        {
            var tenant = _tenantAccessor.Context;
            var botConfig = _configuration.Get<WeiXinWorkConfiguration>(tenant.TenantId);

            var maxRecordCount = 10;

            postModel.Token = botConfig.WeixinCorpToken;
            postModel.EncodingAESKey = botConfig.WeixinCorpEncodingAESKey;
            postModel.CorpId = botConfig.WeixinCorpId;

            await AccessTokenContainer.RegisterAsync(botConfig.WeixinCorpId, botConfig.WeixinCorpSecret);

            #region 用于生产环境测试原始数据

            //_logger.LogInformation($"收到消息 {Request.GetRequestString()}\r\n{postModel.Serialize()}");
            #endregion

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new WorkCustomMessageHandler(botConfig, Request.GetRequestMemoryStream(), postModel, maxRecordCount);

            if (messageHandler.RequestMessage == null)
            {
                //验证不通过或接受信息有错误
                return Content("验证不通过或接受信息有错误");
            }

            try
            {
                //测试时可开启此记录，帮助跟踪数据，使用前请确保App_Data文件夹存在，且有读写权限。
                messageHandler.SaveRequestMessageLog();//记录 Request 日志（可选）
                var cancelSource = new CancellationTokenSource();
                await messageHandler.ExecuteAsync(cancelSource.Token);//执行微信处理过程（关键）

                messageHandler.SaveResponseMessageLog();//记录 Response 日志（可选）

                //自动返回加密后结果
                return new FixWeiXinBugResult(messageHandler);//为了解决官方微信5.0软件换行bug暂时添加的方法，平时用下面一个方法即可
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"ExecptionMessage:{ex.Message}");
                return Content("");
            }
        }

        /*
        /// <summary>
        /// 这是一个最简洁的过程演示
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("MiniPost")]
        public async Task<ActionResult> MiniPost(PostModel postModel)
        {
            var maxRecordCount = 10;

            postModel.Token = _workSetting.WeixinCorpToken;
            postModel.EncodingAESKey = _workSetting.WeixinCorpEncodingAESKey;
            postModel.CorpId = _workSetting.WeixinCorpId;

            //自定义MessageHandler，对微信请求的详细判断操作都在这里面。
            var messageHandler = new WorkCustomMessageHandler(_workSetting, Request.GetRequestMemoryStream(), postModel, maxRecordCount);
            var cancelSource = new CancellationTokenSource();
            //执行微信处理过程
            await messageHandler.ExecuteAsync(cancelSource.Token);
            //自动返回加密后结果
            return new FixWeiXinBugResult(messageHandler);
        }*/
    }
}
