using System;
using Newtonsoft.Json;

namespace E.Common.Serialization
{
    /// <summary>
    /// TimeSpan转换器
    /// </summary>
    public sealed class TimeSpanConverter : JsonConverter<TimeSpan>

    {
        public override void WriteJson(JsonWriter writer, TimeSpan value, JsonSerializer serializer)
        {
            writer.WriteValue($"{value.TotalSeconds}");
        }

        public override TimeSpan ReadJson(JsonReader reader, Type objectType, TimeSpan existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            if (reader?.Value != null)
            {
                var strValue = reader.Value.ToString();
                double.TryParse(strValue, out var second);
                return TimeSpan.FromSeconds(second);
            }

            return TimeSpan.Zero;
        }
    }
}
