using E.Bots.Configuration;
using Senparc.Weixin.Entities;

namespace E.Bots.WeiXin
{
    public class WeiXinWorkConfiguration:IBotConfiguration, ISenparcWeixinSettingForWork
    {
        public string TenantId { get; set; }
        public string ItemKey { get; set; }
        public string WeixinCorpId { get; set; }
        public string WeixinCorpAgentId { get; set; }
        public string WeixinCorpSecret { get; set; }
        public string WeixinCorpToken { get; set; }
        public string WeixinCorpEncodingAESKey { get; set; }
    }
}
