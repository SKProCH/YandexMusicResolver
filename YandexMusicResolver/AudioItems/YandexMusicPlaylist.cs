using System.Collections.ObjectModel;

namespace YandexMusicResolver.AudioItems {
    /// <summary>
    /// Represents playlist from Yandex Music
    /// </summary>
    public class YandexMusicPlaylist : IAudioItem {
        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicPlaylist"/> class.
        /// </summary>
        /// <param name="title">Playlist title</param>
        /// <param name="tracks">Collection with tracks</param>
        /// <param name="isSearchResult">Is this playlist is search result</param>
        public YandexMusicPlaylist(string title, ReadOnlyCollection<YandexMusicTrack> tracks, bool isSearchResult) {
            Title = title;
            Tracks = tracks;
            IsSearchResult = isSearchResult;
        }

        /// <summary>
        /// Playlist title
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Collection with tracks in playlist
        /// </summary>
        public ReadOnlyCollection<YandexMusicTrack> Tracks { get; }

        /// <summary>
        /// Is this playlist a search result
        /// </summary>
        public bool IsSearchResult { get; }
    }
}