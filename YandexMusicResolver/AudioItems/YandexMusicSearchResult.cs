using System.Collections.Generic;

namespace YandexMusicResolver.AudioItems {
    /// <summary>
    /// Represents YandexMusic search result
    /// </summary>
    public class YandexMusicSearchResult {
        internal YandexMusicSearchResult(string query,
                                         bool isSearchResult,
                                         YandexSearchType type,
                                         IReadOnlyCollection<YandexMusicAlbum>? albums,
                                         IReadOnlyCollection<YandexMusicPlaylist>? playlists,
                                         IReadOnlyCollection<YandexMusicTrack>? tracks,
                                         int? limit = null) {
            Query = query;
            Limit = limit;
            Type = type;
            Albums = albums;
            Playlists = playlists;
            Tracks = tracks;
            IsSearchResult = isSearchResult;
        }

        /// <summary>
        /// Search query text
        /// </summary>
        public string Query { get; }

        /// <summary>
        /// Tracks limit count.
        /// Will be <code>null</code> if the <see cref="IsSearchResult"/> is <code>false</code>
        /// </summary>
        public int? Limit { get; }

        /// <summary>
        /// Search data type
        /// </summary>
        public YandexSearchType Type { get; }

        /// <summary>
        /// Is this playlist a search result
        /// </summary>
        public bool IsSearchResult { get; }

        /// <summary>
        /// Albums list.
        /// Will be <code>null</code> if the search should not search for albums
        /// </summary>
        public IReadOnlyCollection<YandexMusicAlbum>? Albums { get; set; }

        /// <summary>
        /// Playlists list.
        /// Will be <code>null</code> if the search should not search for playlists
        /// </summary>
        public IReadOnlyCollection<YandexMusicPlaylist>? Playlists { get; set; }

        /// <summary>
        /// Tracks list.
        /// Will be <code>null</code> if the search should not search for tracks
        /// </summary>
        public IReadOnlyCollection<YandexMusicTrack>? Tracks { get; set; }
    }
}
