using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Senparc.NeuChar.MessageHandlers;

namespace E.Bots.WeiXin
{
    /// <summary>
    /// 修复微信换行bug
    /// </summary>
    public class FixWeiXinBugResult : ContentResult
    {
        protected readonly IMessageHandlerDocument MessageHandlerDocument;

        /// <summary>
        /// 这个类型只用于特殊阶段：目前IOS版本微信有换行的bug，\r\n会识别为2行
        /// </summary>
        public FixWeiXinBugResult(IMessageHandlerDocument messageHandlerDocument)
        {
            MessageHandlerDocument = messageHandlerDocument;
        }

        public FixWeiXinBugResult(string content)
        {
            base.Content = content;
        }


        public new string Content
        {
            get
            {
                if (base.Content != null)
                {
                    return base.Content;
                }

                if (MessageHandlerDocument != null)
                {
                    if (MessageHandlerDocument.TextResponseMessage != null)
                    {
                        return MessageHandlerDocument.TextResponseMessage.Replace("\r\n", "\n");
                    }
                }
                return null;
            }
            set => base.Content = value;
        }
        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var content = this.Content;

            if (content == null)
            {
                //使用IMessageHandler输出
                if (MessageHandlerDocument == null)
                {
                    throw new Senparc.Weixin.Exceptions.WeixinException("执行WeiXinResult时提供的MessageHandler不能为Null！", null);
                }
                var finalResponseDocument = MessageHandlerDocument.FinalResponseDocument;


                if (finalResponseDocument == null)
                {
                    //throw new Senparc.Weixin.MP.WeixinException("FinalResponseDocument不能为Null！", null);
                }
                else
                {
                    content = finalResponseDocument.ToString();
                }
            }

            context.HttpContext.Response.ContentType = "text/xml";
            content = (content ?? "").Replace("\r\n", "\n");

            var bytes = Encoding.UTF8.GetBytes(content);

            await context.HttpContext.Response.Body.WriteAsync(bytes, 0, bytes.Length).ConfigureAwait(false);
        }
    }
}
