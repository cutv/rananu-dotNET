using Newtonsoft.Json;

namespace Rananu.Shared.Converter
{
    public class EnumConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)=> objectType == typeof(IEnum<T>);

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
           throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value != null)
                writer.WriteValue(((IEnum<T>)value).Value);
        }
    }
}
