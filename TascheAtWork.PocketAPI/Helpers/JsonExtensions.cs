using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace TascheAtWork.PocketAPI.Helpers
{

  public class BoolConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      writer.WriteValue(((bool)value) ? 1 : 0);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      return reader.Value != null && (reader.Value.ToString() == "1");
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof(bool);
    }
  }


    public class NullableIntConverter : JsonConverter
  {
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
      writer.WriteValue(value);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      int result = 0;
      if (reader.Value != null)
      {
        result = Convert.ToInt32(reader.Value);
      }
      return result;
    }

    public override bool CanConvert(Type objectType)
    {
      return objectType == typeof(int);
    }
  }



  public class ObjectToArrayConverter<T> : CustomCreationConverter<List<T>> where T : new()
  {
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
      List<T> result = new List<T>();
      JObject jObject = JObject.Load(reader);
      T target;

      // Populate the object properties
      foreach (KeyValuePair<string, JToken> item in jObject)
      {
        target = new T();
        serializer.Populate(item.Value.CreateReader(), target);
        result.Add(target);
      }

      return result;
    }

    public override List<T> Create(Type objectType)
    {
      return new List<T>();
    }
  }
}
