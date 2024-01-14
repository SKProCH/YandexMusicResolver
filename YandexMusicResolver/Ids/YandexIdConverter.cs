using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YandexMusicResolver.Ids;

/// <summary>
/// Allows to properly serialize/deserialize the <see cref="YandexId"/> from JSON
/// </summary>
public class YandexIdConverter : JsonConverter<YandexId> {
    /// <inheritdoc />
    public override YandexId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TryGetInt64(out var l)) {
            return new YandexId(l);
        }

        if (reader.TryGetGuid(out var guid)) {
            return new YandexId(guid);
        }

        throw new NotSupportedException("Current value seems doesn't look like any known YandexId type");
    }
    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, YandexId value, JsonSerializerOptions options) {
        switch (value.IdType) {
            case YandexId.YandexIdType.Long:
                writer.WriteNumberValue(value.LongId);
                break;
            case YandexId.YandexIdType.Guid:
                writer.WriteStringValue(value.GuidId);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(value.IdType), "Current YandexId type can't be wrote to JSON");
        }
    }
}
