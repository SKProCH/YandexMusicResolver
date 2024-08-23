using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Config;
using YandexMusicResolver.Responses;

namespace YandexMusicResolver.Loaders;

/// <inheritdoc />
public class YandexMusicSearchResultLoader : IYandexMusicSearchResultLoader {
    /// <summary>
    /// Default limit for searching
    /// </summary>
    public const int DefaultLimit = 10;

    private const string TracksInfoFormat = "https://api.music.yandex.net/search?type={0}&page=0&text={1}";
    private Regex _searchRegex = new Regex($"ymsearch(:([a-zA-Z]+))?(:([0-9]+))?:([^:]+)");
    private readonly IYandexCredentialsProvider _credentialsProvider;
    private string _searchPrefix = "ymsearch";
    private readonly IYandexMusicPlaylistLoader _playlistLoader;
    private HttpClient _httpClient;

    /// <inheritdoc />
    public string SearchPrefix => _searchPrefix;

    /// <summary>
    /// Initializes a new instance of the <see cref="YandexMusicSearchResultLoader"/> class.
    /// </summary>
    /// <param name="credentialsProvider">Config instance for performing requests</param>
    /// <param name="httpClientFactory">Factory for resolving HttpClient. Client name is <see cref="YandexMusicUtilities.HttpClientName"/></param>
    /// <param name="playlistLoader">Playlist loader instance for resolving albums and playlists</param>
    public YandexMusicSearchResultLoader(IYandexCredentialsProvider credentialsProvider, IHttpClientFactory httpClientFactory, IYandexMusicPlaylistLoader playlistLoader)
        : this(credentialsProvider, httpClientFactory.GetYMusicHttpClient(), playlistLoader) { }

    private YandexMusicSearchResultLoader(IYandexCredentialsProvider credentialsProvider, HttpClient httpClient, IYandexMusicPlaylistLoader playlistLoader) {
        _httpClient = httpClient;
        _playlistLoader = playlistLoader;
        _credentialsProvider = credentialsProvider;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="YandexMusicSearchResultLoader"/> class.
    /// </summary>
    /// <param name="credentialsProvider">Config instance for performing requests</param>
    /// <param name="httpClient">HttpClient for performing requests. But preferred way is use another ctor and pass <see cref="IHttpClientFactory"/></param>
    /// <param name="playlistLoader">Playlist loader instance for resolving albums and playlists</param>
    public static YandexMusicSearchResultLoader CreateWithHttpClient(IYandexCredentialsProvider credentialsProvider, HttpClient httpClient, IYandexMusicPlaylistLoader playlistLoader) {
        return new YandexMusicSearchResultLoader(credentialsProvider, httpClient, playlistLoader);
    }

    /// <inheritdoc />
    public void SetSearchPrefix(string? prefix = null) {
        _searchPrefix = prefix ?? "ymsearch";
        _searchRegex = new Regex($"{_searchPrefix}(:([a-zA-Z]+))?(:([0-9]+))?:([^:]+)");
    }

    /// <inheritdoc />
    public Task<YandexMusicSearchResult?> LoadSearchResult(string query) {
        TryParseQuery(query, out var text, out var type, out var limit);
        return LoadSearchResult(type, text, limit);
    }

    /// <inheritdoc />
    public bool TryParseQuery(string query, out string text, out YandexSearchType type, out int limit) {
        type = YandexSearchType.Track;
        limit = DefaultLimit;
        text = query;

        if (!query.StartsWith(SearchPrefix + ":")) return false;
        var match = _searchRegex.Match(query);
        if (!match.Success) return false;

        type = Enum.TryParse<YandexSearchType>(match.Groups[2].Value, true, out var o) ? o : YandexSearchType.Track;
        limit = int.TryParse(match.Groups[4].Value, out var i) ? i : DefaultLimit;
        text = match.Groups[5].Value;
        return true;
    }

    /// <inheritdoc />
    public async Task<YandexMusicSearchResult?> LoadSearchResult(YandexSearchType type, string query, int limit = DefaultLimit) {
        try {
            var url = string.Format(TracksInfoFormat, type, query);
            var searchResponse = await _httpClient.PerformYMusicRequestAsync<MetaSearchResponse>(_credentialsProvider, url);
            var albums = searchResponse.Albums?.Results.Take(limit).Select(signature => signature.ToYmAlbum(_playlistLoader));
            var playlists = searchResponse.Playlists?.Results.Take(limit).Select(signature => signature.ToYaPlaylist(_playlistLoader));
            var tracks = searchResponse.Tracks?.Results.Take(limit).Select(track => track.ToYmTrack());

            return new YandexMusicSearchResult(query, true, type,
                albums?.ToList().AsReadOnly(),
                playlists?.ToList().AsReadOnly(),
                tracks?.ToList().AsReadOnly(),
                limit);
        }
        catch (Exception e) {
            throw new YandexMusicException("Exception while loading search results", e);
        }
    }
}