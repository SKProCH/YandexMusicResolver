using Newtonsoft.Json;

namespace YandexMusicResolver.Responses {
    internal class MetaPlaylistTrackContainer {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("track")]
        public MetaTrack Track { get; set; } = null!;
    }
}