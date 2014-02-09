using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TascheAtWork.PocketAPI.Helpers
{
    public class UnixDateTimeConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var epoc = new DateTime(1970, 1, 1);
            var delta = (DateTime)value - epoc;

            writer.WriteValue((long)delta.TotalSeconds);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value.ToString() == "0")
                return null;

            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Convert.ToDouble(reader.Value)).ToLocalTime();
        }
    }
}