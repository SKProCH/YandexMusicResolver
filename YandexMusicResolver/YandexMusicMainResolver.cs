﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Config;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver {
    /// <inheritdoc />
    public sealed class YandexMusicMainResolver : IYandexMusicMainResolver {
        private const string TrackUrlPattern = "^https?://music\\.yandex\\.[a-zA-Z]+/album/([0-9]+)/track/([0-9]+)$";
        private const string AlbumUrlPattern = "^https?://music\\.yandex\\.[a-zA-Z]+/album/([0-9]+)$";
        private const string PlaylistUrlPattern = "^https?://music\\.yandex\\.[a-zA-Z]+/users/(.+)/playlists/([0-9]+)$";

        private static readonly Regex TrackUrlRegex = new(TrackUrlPattern);
        private static readonly Regex AlbumUrlRegex = new(AlbumUrlPattern);
        private static readonly Regex PlaylistUrlRegex = new(PlaylistUrlPattern);

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly IYandexConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicMainResolver"/> class.
        /// </summary>
        /// <param name="config">Yandex config instance</param>
        /// <param name="playlistLoader">Instance of <see cref="YandexMusicPlaylistLoader"/></param>
        /// <param name="trackLoader">Instance of <see cref="YandexMusicTrackLoader"/></param>
        /// <param name="directUrlLoader">Instance of <see cref="YandexMusicDirectUrlLoader"/></param>
        /// <param name="searchResultLoader">Instance of <see cref="YandexMusicSearchResultLoader"/></param>
        public YandexMusicMainResolver(IYandexConfig config,
                                       IYandexMusicPlaylistLoader? playlistLoader = null,
                                       IYandexMusicTrackLoader? trackLoader = null,
                                       IYandexMusicDirectUrlLoader? directUrlLoader = null,
                                       IYandexMusicSearchResultLoader? searchResultLoader = null) {
            config.Load();
            _config = config;
            TrackLoader = trackLoader ??= new YandexMusicTrackLoader(_config);
            DirectUrlLoader = directUrlLoader ??= new YandexMusicDirectUrlLoader(_config);
            PlaylistLoader = playlistLoader ??= new YandexMusicPlaylistLoader(_config, trackLoader);
            SearchResultLoader = searchResultLoader ??= new YandexMusicSearchResultLoader(_config, playlistLoader);
        }

        /// <inheritdoc />
        public IYandexMusicPlaylistLoader PlaylistLoader { get; }

        /// <inheritdoc />
        public IYandexMusicTrackLoader TrackLoader { get; }

        /// <inheritdoc />
        public IYandexMusicDirectUrlLoader DirectUrlLoader { get; }

        /// <inheritdoc />
        public IYandexMusicSearchResultLoader SearchResultLoader { get; }

        /// <inheritdoc />
        public bool AllowSearch { get; set; } = true;

        /// <inheritdoc />
        public YandexSearchType PlainTextIsSearchQueryType { get; set; } = YandexSearchType.Track;

        /// <inheritdoc />
        public bool PlainTextIsSearchQuery { get; set; } = true;

        /// <inheritdoc />
        public async Task<YandexMusicSearchResult?> ResolveQuery(string query, bool? allowSearchOverride = null,
                                                                 bool? plainTextIsSearchQueryOverride = null,
                                                                 YandexSearchType? plainTextAsSearchQueryTypeOverride = null) {
            var trackMatch = TrackUrlRegex.Match(query);
            if (trackMatch.Success) {
                var tracks = new List<YandexMusicTrack>();

                var yandexMusicTrack = await TrackLoader.LoadTrack(Convert.ToInt64(trackMatch.Groups[2].Value));
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

        /// <inheritdoc />
        public bool CanResolveQuery(string query, bool? allowSearchOverride = null,
                                    bool? plainTextIsSearchQueryOverride = null,
                                    YandexSearchType? plainTextAsSearchQueryTypeOverride = null) {
            var trackMatch = TrackUrlRegex.Match(query);
            if (trackMatch.Success) return true;

            var playlistMatch = PlaylistUrlRegex.Match(query);
            if (playlistMatch.Success) return true;

            var albumMatch = AlbumUrlRegex.Match(query);
            if (albumMatch.Success) return true;

            if (!(allowSearchOverride ?? AllowSearch)) return false;
            var needSearch = plainTextIsSearchQueryOverride ?? PlainTextIsSearchQuery;
            return SearchResultLoader.TryParseQuery(query, out _, out _, out _) || needSearch;
        }
    }
}
