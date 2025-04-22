using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameCenter.API.Converters
{
    public static class PolymorphicJsonConverterFactory
    {
        public static PolymorphicJsonConverterBuilder<TBase> For<TBase>() =>
            new PolymorphicJsonConverterBuilder<TBase>();
    }

    public class PolymorphicJsonConverterBuilder<TBase>
    {
        private readonly HashSet<Type> _knownTypes = new();

        public PolymorphicJsonConverterBuilder<TBase> Add<TDerived>() where TDerived : TBase
        {
            _knownTypes.Add(typeof(TDerived));
            return this;
        }

        public JsonConverter<TBase> Build()
        {
            return new SimplePolymorphicJsonConverter<TBase>(_knownTypes.ToArray());
        }

        public PolymorphicJsonConverterBuilder<TBase, TDiscriminator> WithDiscriminator<TDiscriminator>(
            Expression<Func<TBase, TDiscriminator>> selector)
        {
            return new PolymorphicJsonConverterBuilder<TBase, TDiscriminator>(selector);
        }
    }

    public class PolymorphicJsonConverterBuilder<TBase, TDiscriminator>
    {
        private readonly Func<TBase, TDiscriminator> _getDiscriminator;
        private readonly string _discriminatorProperty;
        private readonly Dictionary<TDiscriminator, Type> _typeMap = new();

        public PolymorphicJsonConverterBuilder(Expression<Func<TBase, TDiscriminator>> selector)
        {
            _getDiscriminator = selector.Compile();
            _discriminatorProperty = (selector.Body as MemberExpression)?.Member?.Name
                ?? throw new ArgumentException("Invalid property expression for discriminator.");
        }

        public PolymorphicJsonConverterBuilder<TBase, TDiscriminator> Add<TDerived>(TDiscriminator discriminator)
            where TDerived : TBase
        {
            if (_typeMap.ContainsKey(discriminator))
                throw new InvalidOperationException($"Duplicate discriminator value: '{discriminator}'");

            _typeMap[discriminator] = typeof(TDerived);
            return this;
        }

        public JsonConverter<TBase> Build()
        {
            return new PolymorphicJsonConverter<TBase, TDiscriminator>(
                _discriminatorProperty,
                _typeMap,
                _getDiscriminator
            );
        }
    }

    public class PolymorphicJsonConverter<TBase, TDiscriminator> : JsonConverter<TBase>
    {
        private readonly string _discriminatorProperty;
        private readonly Dictionary<TDiscriminator, Type> _typeMap;
        private readonly Func<TBase, TDiscriminator> _getDiscriminator;

        public PolymorphicJsonConverter(
            string discriminatorProperty,
            Dictionary<TDiscriminator, Type> typeMap,
            Func<TBase, TDiscriminator> getDiscriminator)
        {
            _discriminatorProperty = discriminatorProperty;
            _typeMap = typeMap;
            _getDiscriminator = getDiscriminator;
        }

        public override TBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);

            if (!doc.RootElement.TryGetProperty(_discriminatorProperty, out var discriminatorElement))
                throw new JsonException($"Missing discriminator property '{_discriminatorProperty}'");

            var discriminatorValue = JsonSerializer.Deserialize<TDiscriminator>(discriminatorElement.GetRawText(), options)!;
            if (!_typeMap.TryGetValue(discriminatorValue, out var derivedType))
                throw new JsonException($"Unknown discriminator value '{discriminatorValue}'");

            return (TBase)JsonSerializer.Deserialize(doc.RootElement.GetRawText(), derivedType, options)!;
        }

        public override void Write(Utf8JsonWriter writer, TBase value, JsonSerializerOptions options)
        {
            var discriminatorValue = _getDiscriminator(value);
            var type = value!.GetType();

            using var jsonDoc = JsonDocument.Parse(JsonSerializer.Serialize(value, type, options));

            writer.WriteStartObject();
            writer.WritePropertyName(_discriminatorProperty);
            JsonSerializer.Serialize(writer, discriminatorValue, options);

            foreach (var prop in jsonDoc.RootElement.EnumerateObject())
            {
                if (prop.NameEquals(_discriminatorProperty)) continue;
                prop.WriteTo(writer);
            }

            writer.WriteEndObject();
        }
    }

    public class SimplePolymorphicJsonConverter<TBase> : JsonConverter<TBase>
    {
        private readonly Dictionary<string, Type> _typeMap;
        private readonly string _typeDiscriminator = "Type";

        public SimplePolymorphicJsonConverter(params Type[] knownTypes)
        {
            _typeMap = knownTypes.ToDictionary(t => t.Name, t => t);
        }

        public override TBase Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);

            if (!doc.RootElement.TryGetProperty(_typeDiscriminator, out var typeElement))
                throw new JsonException($"Missing discriminator '{_typeDiscriminator}'");

            var typeName = typeElement.GetString();
            if (!_typeMap.TryGetValue(typeName!, out var targetType))
                throw new JsonException($"Unknown type discriminator '{typeName}'");

            return (TBase)JsonSerializer.Deserialize(doc.RootElement.GetRawText(), targetType, options)!;
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
}
