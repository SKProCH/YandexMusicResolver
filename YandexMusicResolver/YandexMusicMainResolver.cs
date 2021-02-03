using System;
using System.Collections.Generic;
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

        private static readonly Regex TrackUrlRegex = new(TrackUrlPattern);
        private static readonly Regex AlbumUrlRegex = new(AlbumUrlPattern);
        private static readonly Regex PlaylistUrlRegex = new(PlaylistUrlPattern);

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
            config.Load();
            _config = config;
            PlaylistLoader = playlistLoader ?? new YandexMusicPlaylistLoader(_config);
            TrackLoader = trackLoader ?? new YandexMusicTrackLoader(_config);
            DirectUrlLoader = directUrlLoader ?? new YandexMusicDirectUrlLoader(_config);
            SearchResultLoader = searchResultLoader ?? new YandexMusicSearchResultLoader(_config, PlaylistLoader);
        }

        /// <summary>
        /// Is complicated query in <see cref="ResolveQuery"/> can be resolved
        /// </summary>
        public bool AllowSearch { get; set; } = true;

        /// <summary>
        /// If we pass plain text to <see cref="ResolveQuery"/> it will be interpreted as search query with this search type. Set <see cref="PlainTextIsSearchQuery"/> to <code>false</code> to disable this
        /// </summary>
        public YandexSearchType PlainTextIsSearchQueryType { get; set; } = YandexSearchType.Track;

        /// <summary>
        /// Will plain text be interpreted as a search query in <see cref="ResolveQuery"/>
        /// </summary>
        public bool PlainTextIsSearchQuery { get; set; } = true;

        /// <summary>
        /// Resolves yandex query. Can directly resolve playlists, albums, tracks by url and search queries
        /// </summary>
        /// <param name="query">Direct url or search query</param>
        /// <param name="allowSearchOverride">Is query in <see cref="ResolveQuery"/> can be resolved with search. This parameter overrides <see cref="AllowSearch"/></param>
        /// <param name="plainTextIsSearchQueryOverride">Will plain text be interpreted as a search query in <see cref="ResolveQuery"/></param>
        /// <param name="plainTextAsSearchQueryTypeOverride">If we pass plain text to <see cref="ResolveQuery"/> it will be interpreted as search query with this search type</param>
        /// <returns>Instance of <see cref="YandexMusicSearchResult"/>. Null if track will now an valid url and <see cref="AllowSearch"/> is false.</returns>
        public async Task<YandexMusicSearchResult?> ResolveQuery(string query, bool? allowSearchOverride = null,
                                                                 bool? plainTextIsSearchQueryOverride = null,
                                                                 YandexSearchType? plainTextAsSearchQueryTypeOverride = null) {
            var trackMatch = TrackUrlRegex.Match(query);
            if (trackMatch.Success) {
                var tracks = new List<YandexMusicTrack>();

                var yandexMusicTrack = await TrackLoader.LoadTrack(trackMatch.Groups[2].Value);
                if (yandexMusicTrack != null) tracks.Add(yandexMusicTrack);

                return new YandexMusicSearchResult(query, false, YandexSearchType.Track, null, null, tracks.AsReadOnly());
            }

            var playlistMatch = PlaylistUrlRegex.Match(query);
            if (playlistMatch.Success) {
                var playlists = new List<YandexMusicPlaylist>();

                var playlist = await PlaylistLoader.LoadPlaylist(playlistMatch.Groups[1].Value, playlistMatch.Groups[2].Value);
                if (playlist != null) playlists.Add(playlist);

                return new YandexMusicSearchResult(query, false, YandexSearchType.Playlist, null, playlists.AsReadOnly(), null);
            }

            var albumMatch = AlbumUrlRegex.Match(query);
            if (albumMatch.Success) {
                var albums = new List<YandexMusicAlbum>();

                var album = await PlaylistLoader.LoadAlbum(albumMatch.Groups[1].Value);
                if (album != null) albums.Add(album);

                return new YandexMusicSearchResult(query, false, YandexSearchType.Album, albums.AsReadOnly(), null, null);
            }

            if (!(allowSearchOverride ?? AllowSearch)) return null;
            string searchText = query;
            var searchType = plainTextAsSearchQueryTypeOverride ?? PlainTextIsSearchQueryType;
            var searchLimit = 10;
            var needSearch = plainTextIsSearchQueryOverride ?? PlainTextIsSearchQuery;
            if (SearchResultLoader.TryParseQuery(query, out var text, out var type, out var limit)) {
                searchText = text;
                searchType = type;
                searchLimit = limit;
                needSearch = true;
            }

            if (needSearch) return await SearchResultLoader.LoadSearchResult(searchType, searchText, searchLimit);
            return null;
        }
    }
}