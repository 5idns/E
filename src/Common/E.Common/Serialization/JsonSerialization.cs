using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace E.Common.Serialization
{
    /// <summary>
    /// JSON序列化
    /// </summary>
    public static class JsonSerialization
    {
        private static readonly JsonSerializerSettings Settings;
        private static readonly JsonSerializerSettings TypeNameSettings;

        static JsonSerialization()
        {
            Settings = new JsonSerializerSettings
            {
                MaxDepth = 10,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                CheckAdditionalContent = false,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                TypeNameHandling = TypeNameHandling.Auto,
                Converters = new List<JsonConverter>
                {
                    new IsoDateTimeConverter {DateTimeFormat = "yyyy-MM-dd HH:mm:ss"},
                    new TimeSpanConverter()
                }
            };
            TypeNameSettings = new JsonSerializerSettings
            {
                MaxDepth = 10,
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                CheckAdditionalContent = false,
                DateFormatString = "yyyy-MM-dd HH:mm:ss",
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                Converters = new List<JsonConverter>
                {
                    new IsoDateTimeConverter {DateTimeFormat = "yyyy-MM-dd HH:mm:ss"},
                    new TimeSpanConverter()
                }
            };
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="input">序列化对象</param>
        /// <returns>JSON字符串</returns>
        public static string Serialize<T>(this T input)
        {
            return input == null ? string.Empty : JsonConvert.SerializeObject(input, Settings);
        }

        /// <summary>
        /// 序列化，包含对象类型
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="input">序列化对象</param>
        /// <returns>JSON字符串</returns>
        public static string SerializeHasTypeName<T>(this T input)
        {
            return input == null ? string.Empty : JsonConvert.SerializeObject(input, TypeNameSettings);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="input">序列化对象</param>
        /// <returns>JSON字符串</returns>
        public static string Serialize(this object input)
        {
            return input == null ? string.Empty : JsonConvert.SerializeObject(input, Settings);
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="input">JSON字符串</param>
        /// <returns>序列化对象</returns>
        public static T Deserialize<T>(this string input)
        {
            return string.IsNullOrWhiteSpace(input) ? default : JsonConvert.DeserializeObject<T>(input, Settings);
        }
        /// <summary>
        /// 反序列化，包含对象类型
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="input">JSON字符串</param>
        /// <returns>序列化对象</returns>
        public static T DeserializeHasTypeName<T>(this string input)
        {
            return string.IsNullOrWhiteSpace(input) ? default : JsonConvert.DeserializeObject<T>(input, TypeNameSettings);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="input">JSON字符串</param>
        /// <param name="type">对象类型</param>
        /// <returns>序列化对象</returns>
        public static object Deserialize(this string input, Type type)
        {
            return string.IsNullOrWhiteSpace(input) ? default : JsonConvert.DeserializeObject(input, type);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="input">序列化对象</param>
        /// <param name="serializerSettings">序列化设置</param>
        /// <returns>JSON字符串</returns>
        public static string Serialize<T>(this T input, JsonSerializerSettings serializerSettings)
        {
            return input == null ? string.Empty : JsonConvert.SerializeObject(input, serializerSettings);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="input">序列化对象</param>
        /// <param name="serializerSettings">序列化设置</param>
        /// <returns>JSON字符串</returns>
        public static string Serialize(this object input, JsonSerializerSettings serializerSettings)
        {
            return input == null ? string.Empty : JsonConvert.SerializeObject(input, serializerSettings);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="input">JSON字符串</param>
        /// <param name="serializerSettings">序列化设置</param>
        /// <returns>序列化对象</returns>
        public static T Deserialize<T>(this string input, JsonSerializerSettings serializerSettings)
        {
            return string.IsNullOrWhiteSpace(input)
                ? default
                : JsonConvert.DeserializeObject<T>(input, serializerSettings);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="input">JSON字符串</param>
        /// <param name="type">对象类型</param>
        /// <param name="serializerSettings">序列化设置</param>
        /// <returns>序列化对象</returns>
        public static object Deserialize(this string input, Type type, JsonSerializerSettings serializerSettings)
        {
            return string.IsNullOrWhiteSpace(input)
                ? default
                : JsonConvert.DeserializeObject(input, type, serializerSettings);
        }
    }
}
