using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace E.Bots
{
    public static class RequestUtility
    {
        /// <summary>
        /// 从 Request.Body 中读取流，并复制到一个独立的 MemoryStream 对象中
        /// </summary>
        /// <param name="request"></param>
        /// <param name="allowSynchronousIO"></param>
        /// <returns></returns>
        public static Stream GetRequestMemoryStream(this HttpRequest request, bool? allowSynchronousIO = true)
        {
#if NETSTANDARD2_1
            var syncIOFeature = request.HttpContext.Features.Get<IHttpBodyControlFeature>();

            if (syncIOFeature != null && allowSynchronousIO.HasValue)
            {
                syncIOFeature.AllowSynchronousIO = allowSynchronousIO.Value;
            }
#endif
            string body = new StreamReader(request.Body).ReadToEnd();
            byte[] requestData = Encoding.UTF8.GetBytes(body);
            Stream inputStream = new MemoryStream(requestData);
            return inputStream;
        }

        /// <summary>
        /// 从 Request.Body 中读取流，并转换成字符串
        /// </summary>
        /// <param name="request"></param>
        /// <param name="allowSynchronousIO"></param>
        /// <returns></returns>
        public static string GetRequestString(this HttpRequest request, bool? allowSynchronousIO = true)
        {
#if NETSTANDARD2_1
            var syncIOFeature = request.HttpContext.Features.Get<IHttpBodyControlFeature>();

            if (syncIOFeature != null && allowSynchronousIO.HasValue)
            {
                syncIOFeature.AllowSynchronousIO = allowSynchronousIO.Value;
            }
#endif
            string body = new StreamReader(request.Body).ReadToEnd();
            byte[] requestData = Encoding.UTF8.GetBytes(body);
            return Encoding.UTF8.GetString(requestData);
        }
    }
}
