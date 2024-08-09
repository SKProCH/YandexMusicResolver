using System.Text.Json.Serialization;
using YandexMusicResolver.Ids;

namespace YandexMusicResolver.Responses {
    internal class MetaPlaylistTrackContainer {
        [JsonConverter(typeof(YandexIdConverter))]
        public YandexId Id { get; set; }

        public MetaTrack? Track { get; set; }
    }
}
