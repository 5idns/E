using System;
using System.IO;
using Senparc.Weixin.Entities;
using Senparc.Weixin.Work.AdvancedAPIs;
using Senparc.Weixin.Work.Containers;
using Senparc.Weixin.Work.Entities;
using Senparc.Weixin.Work.MessageHandlers;

namespace E.Bots.WeiXin
{
    public class WorkCustomMessageHandler : WorkMessageHandler<WorkCustomMessageContext>
    {
        private readonly ISenparcWeixinSettingForWork _workSetting;

        /// <summary>
        /// 为中间件提供生成当前类的委托
        /// </summary>
        public static
            Func<ISenparcWeixinSettingForWork, Stream, PostModel, int, IServiceProvider, WorkCustomMessageHandler>
            GenerateMessageHandler =
                (workSetting, stream, postModel, maxRecordCount, serviceProvider) =>
                    new WorkCustomMessageHandler(workSetting, stream, postModel, maxRecordCount, serviceProvider);


        public WorkCustomMessageHandler(ISenparcWeixinSettingForWork workSetting, Stream inputStream,
            PostModel postModel, int maxRecordCount = 0, IServiceProvider serviceProvider = null)
            : base(inputStream, postModel, maxRecordCount, serviceProvider: serviceProvider)
        {
            _workSetting = workSetting;
        }

        public override IWorkResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您发送了消息：" + requestMessage.Content;

            //发送一条客服消息
            var appKey = AccessTokenContainer.BuildingKey(_workSetting.WeixinCorpId, _workSetting.WeixinCorpSecret);
            MassApi.SendText(appKey, _workSetting.WeixinCorpAgentId, "这是一条客服消息，对应您发送的消息：" + requestMessage.Content,
                OpenId);

            return responseMessage;
        }

        public override IWorkResponseMessageBase OnImageRequest(RequestMessageImage requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageImage>();
            responseMessage.Image.MediaId = requestMessage.MediaId;
            return responseMessage;
        }

        public override IWorkResponseMessageBase OnEvent_PicPhotoOrAlbumRequest(
            RequestMessageEvent_Pic_Photo_Or_Album requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "您刚发送的图片如下：";
            return responseMessage;
        }

        public override IWorkResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = $"位置坐标 {requestMessage.Latitude} - {requestMessage.Longitude}";
            return responseMessage;
        }

        public override IWorkResponseMessageBase OnEvent_EnterAgentRequest(
            RequestMessageEvent_Enter_Agent requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = $"欢迎进入应用！现在时间是：{SystemTime.Now.DateTime}";
            return responseMessage;
        }

        public override IWorkResponseMessageBase DefaultResponseMessage(IWorkRequestMessageBase requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这是一条没有找到合适回复信息的默认消息。";
            return responseMessage;
        }
    }
}
