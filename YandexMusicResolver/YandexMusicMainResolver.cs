using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Config;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver {
    /// <summary>
    /// Represent main class for interacting with Yandex Music
    /// </summary>
    public class YandexMusicMainResolver {
        private const string TrackUrlPattern = "^https?://music\\.yandex\\.[a-zA-Z]+/album/([0-9]+)/track/([0-9]+)$";
        private const string AlbumUrlPattern = "^https?://music\\.yandex\\.[a-zA-Z]+/album/([0-9]+)$";
        private const string PlaylistUrlPattern = "^https?://music\\.yandex\\.[a-zA-Z]+/users/(.+)/playlists/([0-9]+)$";

        private static readonly Regex TrackUrlRegex = new Regex(TrackUrlPattern);
        private static readonly Regex AlbumUrlRegex = new Regex(AlbumUrlPattern);
        private static readonly Regex PlaylistUrlRegex = new Regex(PlaylistUrlPattern);

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly IYandexConfig _config;

        /// <summary>
        /// Instance of <see cref="YandexMusicPlaylistLoader"/>
        /// </summary>
        public virtual YandexMusicPlaylistLoader PlaylistLoader { get; }

        /// <summary>
        /// Instance of <see cref="YandexMusicTrackLoader"/>
        /// </summary>
        public virtual YandexMusicTrackLoader TrackLoader { get; }

        /// <summary>
        /// Instance of <see cref="YandexMusicDirectUrlLoader"/>
        /// </summary>
        public virtual YandexMusicDirectUrlLoader DirectUrlLoader { get; }

        /// <summary>
        /// Instance of <see cref="YandexMusicSearchResultLoader"/>
        /// </summary>
        public virtual YandexMusicSearchResultLoader SearchResultLoader { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicMainResolver"/> class.
        /// </summary>
        /// <param name="config">Yandex config instance</param>
        /// <param name="playlistLoader">Instance of <see cref="YandexMusicPlaylistLoader"/></param>
        /// <param name="trackLoader">Instance of <see cref="YandexMusicTrackLoader"/></param>
        /// <param name="directUrlLoader">Instance of <see cref="YandexMusicDirectUrlLoader"/></param>
        /// <param name="searchResultLoader">Instance of <see cref="YandexMusicSearchResultLoader"/></param>
        public YandexMusicMainResolver(IYandexConfig config,
                                       YandexMusicPlaylistLoader? playlistLoader = null,
                                       YandexMusicTrackLoader? trackLoader = null,
                                       YandexMusicDirectUrlLoader? directUrlLoader = null,
                                       YandexMusicSearchResultLoader? searchResultLoader = null) {
            _config = config;
            PlaylistLoader = playlistLoader ?? new YandexMusicPlaylistLoader(_config);
            TrackLoader = trackLoader ?? new YandexMusicTrackLoader(_config);
            DirectUrlLoader = directUrlLoader ?? new YandexMusicDirectUrlLoader(_config);
            SearchResultLoader = searchResultLoader ?? new YandexMusicSearchResultLoader(_config);
        }

        /// <summary>
        /// Is query in <see cref="ResolveQuery"/> can be resolved with search
        /// </summary>
        public bool AllowSearch { get; set; } = true;

        /// <summary>
        /// Resolves yandex query. Can directly resolve playlists, albums, tracks by url and search queries
        /// </summary>
        /// <param name="query">Direct url or search query</param>
        /// <param name="allowSearchOverride">Is query in <see cref="ResolveQuery"/> can be resolved with search. This parameter overrides <see cref="AllowSearch"/></param>
        /// <returns>Instance of <see cref="IAudioItem"/></returns>
        public async Task<IAudioItem?> ResolveQuery(string query, bool? allowSearchOverride = null) {
            var trackMatch = TrackUrlRegex.Match(query);
            if (trackMatch.Success) {
                return await TrackLoader.LoadTrack(trackMatch.Groups[2].Value, GetTrack);
            }

            var playlistMatch = PlaylistUrlRegex.Match(query);
            if (playlistMatch.Success) {
                return await PlaylistLoader.LoadPlaylist(playlistMatch.Groups[1].Value, playlistMatch.Groups[2].Value, GetTrack);
            }

            var albumMatch = AlbumUrlRegex.Match(query);
            if (albumMatch.Success) {
                return await PlaylistLoader.LoadAlbum(albumMatch.Groups[1].Value, GetTrack);
            }

            if (allowSearchOverride ?? AllowSearch) {
                return await SearchResultLoader.LoadSearchResult(query, PlaylistLoader, GetTrack);
            }

            return null;
        }

        private YandexMusicTrack GetTrack(AudioTrackInfo arg) {
            return new(arg, this);
        }
    }
}