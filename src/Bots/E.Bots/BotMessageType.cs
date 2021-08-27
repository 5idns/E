namespace E.Bots
{
    /// <summary>
    /// 消息类型
    /// </summary>
    public enum BotMessageType
    {
        Default = 0,//默认
        Text = 1, //文本
        Location = 2, //地理位置
        Image = 3, //图片
        Voice = 4, //语音
        Video = 5, //视频
        Link = 6, //连接信息
        Event = 7, //事件推送
        ShortVideo = 8, //小视频
        File = 9,//文件
    }
}
