using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace E.Common.Serialization
{
    /// <summary>
    /// 字节数组序列化
    /// </summary>
    public static class ByteArraySerialization
    {
        /// <summary>
        /// 将对象转换为字节数组
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>序列化后的字节数组</returns>
        public static byte[] ToByteArray<T>(this T obj) where T : class
        {
            if (obj == null)
            {
                return null;
            }
            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                return memoryStream.ToArray();
            }
        }
        /// <summary>
        /// 将字节数据转换为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="byteArray">序列化后的字节数组</param>
        /// <returns>对象</returns>
        public static T FromByteArray<T>(this byte[] byteArray) where T : class
        {
            if (byteArray == null)
            {
                return default;
            }
            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(byteArray))
            {
                return binaryFormatter.Deserialize(memoryStream) as T;
            }
        }
    }
}
