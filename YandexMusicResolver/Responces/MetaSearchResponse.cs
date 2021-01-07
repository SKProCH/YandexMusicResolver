using System.Collections.Generic;
using Newtonsoft.Json;

namespace YandexMusicResolver.Responces {
    public class MetaSearchResponse {
        [JsonProperty("albums")]
        public MetaSearchContentProxy<MetaAlbumSignature>? Albums { get; set; }

        [JsonProperty("playlists")]
        public MetaSearchContentProxy<MetaPlaylistSignature>? Playlists { get; set; }

        [JsonProperty("tracks")]
        public MetaSearchContentProxy<MetaTrack>? Tracks { get; set; }
    }

    public class MetaSearchContentProxy<T> {
        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("perPage")]
        public long PerPage { get; set; }

        [JsonProperty("order")]
        public long Order { get; set; }

        [JsonProperty("results")]
        public List<T> Results { get; set; }
    }
}