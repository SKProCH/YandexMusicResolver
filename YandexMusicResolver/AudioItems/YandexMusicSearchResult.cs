using System.Collections.ObjectModel;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver.AudioItems {
    /// <summary>
    /// Represents YandexMusic search result
    /// </summary>
    public class YandexMusicSearchResult : IAudioItem {
        public YandexMusicSearchResult(string query, int limit, YandexSearchType type, ReadOnlyCollection<MetaAlbumSignature>? albums,
                                       ReadOnlyCollection<MetaPlaylistSignature>? playlists, ReadOnlyCollection<MetaTrack>? tracks) {
            Query = query;
            Limit = limit;
            Type = type;
            Albums = albums;
            Playlists = playlists;
            Tracks = tracks;
        }

        /// <summary>
        /// Search query text
        /// </summary>
        public string Query { get; }

        /// <summary>
        /// Tracks limit count
        /// </summary>
        public int Limit { get; }

        /// <summary>
        /// Search data type
        /// </summary>
        public YandexSearchType Type { get; }

        /// <summary>
        /// Albums list.
        /// Will be <code>null</code> if the search should not search for albums
        /// </summary>
        public ReadOnlyCollection<MetaAlbumSignature>? Albums { get; set; }

        /// <summary>
        /// Playlists list.
        /// Will be <code>null</code> if the search should not search for playlists
        /// </summary>
        public ReadOnlyCollection<MetaPlaylistSignature>? Playlists { get; set; }

        /// <summary>
        /// Tracks list.
        /// Will be <code>null</code> if the search should not search for tracks
        /// </summary>
        public ReadOnlyCollection<MetaTrack>? Tracks { get; set; }
    }
}