using Newtonsoft.Json;

namespace YandexMusicResolver.Responces {
    internal class MetaPlaylistTrackContainer {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("track")]
        public MetaPlaylistTrack Track { get; set; } = null!;
    }
}