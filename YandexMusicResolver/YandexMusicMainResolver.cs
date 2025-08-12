using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Config;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver;

/// <inheritdoc />
public sealed partial class YandexMusicMainResolver : IYandexMusicMainResolver {
    private const string UrlRegexPattern =
        @"^https?://music\.yandex\.[a-zA-Z]+/" + 
        "(?:(?:album/(?<albumId>[0-9]+)/track/(?<trackId>[0-9]+))" +
        "|(?:album/(?<albumOnlyId>[0-9]+))" +
        "|(?:track/(?<trackId>[0-9]+))" +
        "|(?:users/(?<userId>[^/]+)/playlists/(?<playlistId>[0-9]+))" +
        "|(?:playlists/(?<playlistUuid>\\S+)))";
#if NET9_0_OR_GREATER
    [GeneratedRegex(UrlRegexPattern)] private static partial Regex UrlParserRegex { get; }
#else
    private static Regex UrlParserRegex { get; } = new(UrlRegexPattern);
#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="YandexMusicMainResolver"/> class.
    /// </summary>
    /// <param name="playlistLoader">Instance of <see cref="YandexMusicPlaylistLoader"/></param>
    /// <param name="trackLoader">Instance of <see cref="YandexMusicTrackLoader"/></param>
    /// <param name="directUrlLoader">Instance of <see cref="YandexMusicDirectUrlLoader"/></param>
    /// <param name="searchResultLoader">Instance of <see cref="YandexMusicSearchResultLoader"/></param>
    public YandexMusicMainResolver(IYandexMusicPlaylistLoader playlistLoader, IYandexMusicTrackLoader trackLoader,
        IYandexMusicDirectUrlLoader directUrlLoader, IYandexMusicSearchResultLoader searchResultLoader) {
        TrackLoader = trackLoader;
        DirectUrlLoader = directUrlLoader;
        PlaylistLoader = playlistLoader;
        SearchResultLoader = searchResultLoader;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="YandexMusicMainResolver"/> class.
    /// </summary>
    /// <param name="credentialsProvider">Yandex config instance</param>
    /// <param name="clientFactory">Factory for resolving HttpClient. Client name is <see cref="YandexMusicUtilities.HttpClientName"/></param>
    /// <param name="playlistLoader">Instance of <see cref="YandexMusicPlaylistLoader"/></param>
    /// <param name="trackLoader">Instance of <see cref="YandexMusicTrackLoader"/></param>
    /// <param name="directUrlLoader">Instance of <see cref="YandexMusicDirectUrlLoader"/></param>
    /// <param name="searchResultLoader">Instance of <see cref="YandexMusicSearchResultLoader"/></param>
    public static YandexMusicMainResolver Create(IYandexCredentialsProvider credentialsProvider,
        IHttpClientFactory clientFactory, IYandexMusicPlaylistLoader? playlistLoader = null,
        IYandexMusicTrackLoader? trackLoader = null, IYandexMusicDirectUrlLoader? directUrlLoader = null,
        IYandexMusicSearchResultLoader? searchResultLoader = null) =>
        Create(credentialsProvider, clientFactory.GetYMusicHttpClient(),
            playlistLoader, trackLoader, directUrlLoader, searchResultLoader);

    /// <summary>
    /// Initializes a new instance of the <see cref="YandexMusicMainResolver"/> class.
    /// </summary>
    /// <param name="credentialsProvider">Yandex config instance</param>
    /// <param name="client">HttpClient for performing requests. But preferred way is use
    /// <see cref="Create(YandexMusicResolver.Config.IYandexCredentialsProvider,System.Net.Http.IHttpClientFactory,YandexMusicResolver.Loaders.IYandexMusicPlaylistLoader?,YandexMusicResolver.Loaders.IYandexMusicTrackLoader?,YandexMusicResolver.Loaders.IYandexMusicDirectUrlLoader?,YandexMusicResolver.Loaders.IYandexMusicSearchResultLoader?)"/>
    /// and pass <see cref="IHttpClientFactory"/></param>
    /// <param name="playlistLoader">Instance of <see cref="YandexMusicPlaylistLoader"/></param>
    /// <param name="trackLoader">Instance of <see cref="YandexMusicTrackLoader"/></param>
    /// <param name="directUrlLoader">Instance of <see cref="YandexMusicDirectUrlLoader"/></param>
    /// <param name="searchResultLoader">Instance of <see cref="YandexMusicSearchResultLoader"/></param>
    public static YandexMusicMainResolver Create(IYandexCredentialsProvider credentialsProvider,
        HttpClient client,
        IYandexMusicPlaylistLoader? playlistLoader = null,
        IYandexMusicTrackLoader? trackLoader = null,
        IYandexMusicDirectUrlLoader? directUrlLoader = null,
        IYandexMusicSearchResultLoader? searchResultLoader = null) {
        trackLoader ??= YandexMusicTrackLoader.CreateWithHttpClient(credentialsProvider, client);
        directUrlLoader ??= YandexMusicDirectUrlLoader.CreateWithHttpClient(credentialsProvider, client);
        playlistLoader ??= YandexMusicPlaylistLoader.CreateWithHttpClient(credentialsProvider, client, trackLoader);
        searchResultLoader ??=
            YandexMusicSearchResultLoader.CreateWithHttpClient(credentialsProvider, client, playlistLoader);

        return new YandexMusicMainResolver(playlistLoader, trackLoader, directUrlLoader, searchResultLoader);
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
        var match = UrlParserRegex.Match(query);

        if (match.Success) {
            if (match.Groups["trackId"].Success) {
                var tracks = new List<YandexMusicTrack>(1);

                var yandexMusicTrack = await TrackLoader.LoadTrack(Convert.ToInt64(match.Groups["trackId"].Value));
                if (yandexMusicTrack != null) {
                    tracks.Add(yandexMusicTrack);
                }

                return new YandexMusicSearchResult(query, false, YandexSearchType.Track, null, null,
                    tracks.AsReadOnly());
            }

            if (match.Groups["albumOnlyId"].Success) {
                var albums = new List<YandexMusicAlbum>(1);

                var album = await PlaylistLoader.LoadAlbum(match.Groups["albumOnlyId"].Value);
                if (album != null) {
                    albums.Add(album);
                }

                return new YandexMusicSearchResult(query, false, YandexSearchType.Album, albums.AsReadOnly(), null,
                    null);
            }

            if (match.Groups["playlistId"].Success) {
                var playlists = new List<YandexMusicPlaylist>(1);

                var playlist = await PlaylistLoader.LoadPlaylist(
                    match.Groups["userId"].Value, match.Groups["playlistId"].Value);
                if (playlist != null) {
                    playlists.Add(playlist);
                }

                return new YandexMusicSearchResult(query, false, YandexSearchType.Playlist, null,
                    playlists.AsReadOnly(), null);
            }

            if (match.Groups["playlistUuid"].Success) {
                var playlists = new List<YandexMusicPlaylist>();

                var playlist = await PlaylistLoader.LoadPlaylist(match.Groups["playlistUuid"].Value);
                if (playlist != null) {
                    playlists.Add(playlist);
                }

                return new YandexMusicSearchResult(query, false, YandexSearchType.Playlist, null,
                    playlists.AsReadOnly(), null);
            }
        }

        if (!(allowSearchOverride ?? AllowSearch)) {
            return null;
        }

        var searchText = query;
        var searchType = plainTextAsSearchQueryTypeOverride ?? PlainTextIsSearchQueryType;
        var searchLimit = 10;
        var needSearch = plainTextIsSearchQueryOverride ?? PlainTextIsSearchQuery;
        if (SearchResultLoader.TryParseQuery(query, out var text, out var type, out var limit)) {
            searchText = text;
            searchType = type;
            searchLimit = limit;
            needSearch = true;
        }

        if (needSearch) {
            return await SearchResultLoader.LoadSearchResult(searchType, searchText, searchLimit);
        }

        return null;
    }

    /// <inheritdoc />
    public bool CanResolveQuery(string query, bool? allowSearchOverride = null,
        bool? plainTextIsSearchQueryOverride = null,
        YandexSearchType? plainTextAsSearchQueryTypeOverride = null) {
        if (UrlParserRegex.IsMatch(query)) {
            return true;
        }

        if (!(allowSearchOverride ?? AllowSearch)) return false;
        var needSearch = plainTextIsSearchQueryOverride ?? PlainTextIsSearchQuery;
        return SearchResultLoader.TryParseQuery(query, out _, out _, out _) || needSearch;
    }
}
