using System.Text.Json;
using System.Text.Json.Serialization;

namespace Consyzer.Core.Converters;

internal sealed class JsonEnumConverter<T> : JsonConverter<T>
    where T : struct, Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var str = reader.GetString();

        if (Enum.TryParse<T>(str, ignoreCase: true, out var result))
        {
            return result;
        }

        return default;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}