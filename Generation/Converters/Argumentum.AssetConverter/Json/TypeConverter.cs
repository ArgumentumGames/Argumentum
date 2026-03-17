using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Argumentum.AssetConverter.Json
{
    public class TypeConverter : JsonConverter<Type>
    {
        public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string typeName = reader.GetString();
            if (string.IsNullOrEmpty(typeName))
            {
                return null;
            }
            return Type.GetType(typeName, true);
        }

        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.AssemblyQualifiedName);
        }
    }
}