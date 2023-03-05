using System.Collections.Generic;

namespace YandexMusicResolver.Responses {
    internal class MetaSearchResponse {
        public MetaSearchContentProxy<MetaAlbumSignature>? Albums { get; set; }

        public MetaSearchContentProxy<MetaPlaylistSignature>? Playlists { get; set; }

        public MetaSearchContentProxy<MetaTrack>? Tracks { get; set; }
    }

    internal class MetaSearchContentProxy<T> {
        public long Total { get; set; }

        public long PerPage { get; set; }

        public long Order { get; set; }

        public List<T> Results { get; set; } = null!;
    }
}
