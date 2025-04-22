using System.Text.Json;
using System.Text.Json.Serialization;

public class PolymorphicJsonConverter<TBase> : JsonConverter<TBase>
{
    private readonly Dictionary<string, Type> _typeMap;
    private readonly string _typeDiscriminator;

    public PolymorphicJsonConverter(string typeDiscriminator = "Type", params Type[] knownTypes)
    {
        _typeDiscriminator = typeDiscriminator;
        _typeMap = knownTypes.ToDictionary(
            t => t.Name,
            t => t
        );
    }

    public override TBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using JsonDocument document = JsonDocument.ParseValue(ref reader);
        if (!document.RootElement.TryGetProperty(_typeDiscriminator, out var typeElement))
            throw new JsonException($"Missing discriminator '{_typeDiscriminator}'");

        var typeName = typeElement.GetString();
        if (!_typeMap.TryGetValue(typeName, out var targetType))
            throw new JsonException($"Unknown type discriminator '{typeName}'");

        string rawJson = document.RootElement.GetRawText();
        return (TBase)JsonSerializer.Deserialize(rawJson, targetType, options)!;
    }

    public override void Write(Utf8JsonWriter writer, TBase value, JsonSerializerOptions options)
    {
        var type = value!.GetType();
        using var jsonDoc = JsonDocument.Parse(JsonSerializer.Serialize(value, type, options));

        writer.WriteStartObject();
        writer.WriteString(_typeDiscriminator, type.Name);

        foreach (var prop in jsonDoc.RootElement.EnumerateObject())
        {
            prop.WriteTo(writer);
        }

        writer.WriteEndObject();
    }
}
