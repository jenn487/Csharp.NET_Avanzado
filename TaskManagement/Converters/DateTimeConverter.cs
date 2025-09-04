using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TaskManagement.API.Converters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        private const string Format = "dd-MM-yyyy hh:mm tt"; 

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }
}
