using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Config;
using YandexMusicResolver.Requests;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver.Loaders {
    /// <summary>
    /// Represents search on Yandex Music
    /// </summary>
    public class YandexMusicSearchResultLoader {
        /// <summary>
        /// Default limit for searching
        /// </summary>
        public const int DefaultLimit = 10;

        private const string TracksInfoFormat = "https://api.music.yandex.net/search?type={0}&page=0&text={1}";
        private Regex SearchRegex;
        private IYandexConfig _config;

        /// <summary>
        /// Special prefix for complicated requests
        /// </summary>
        public string SearchPrefix { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicSearchResultLoader"/> class.
        /// </summary>
        /// <param name="config">Config instance for performing requests</param>
        /// <param name="searchPrefix"></param>
        public YandexMusicSearchResultLoader(IYandexConfig config, string? searchPrefix = null) {
            // ReSharper disable once StringLiteralTypo
            SearchPrefix = searchPrefix ?? "ymsearch";
            _config = config;
            SearchRegex = new Regex($"{SearchPrefix}(:([a-zA-Z]+))?(:([0-9]+))?:([^:]+)");
        }

        /// <summary>
        /// Perform search request on Yandex Music
        /// </summary>
        /// <remarks>Complicated query is <see cref="SearchPrefix"/>:<see cref="YandexSearchType"/>:limit:text</remarks>
        /// <param name="query">Search query. May be complicated or default values will be used</param>
        /// <param name="playlistLoader">Playlist loader instance</param>
        /// <param name="trackFactory">Track factory to create YandexMusicTrack from AudioTrackInfo</param>
        /// <returns>Instance of YandexMusicSearchResult</returns>
        public Task<YandexMusicSearchResult?> LoadSearchResult(string query, YandexMusicPlaylistLoader playlistLoader,
                                                               Func<AudioTrackInfo, YandexMusicTrack> trackFactory) {
            TryParseQuery(query, out var text, out var type, out var limit);
            return LoadSearchResult(type, text, playlistLoader, trackFactory, limit);
        }

        /// <summary>
        /// Parse complicated query into pieces
        /// </summary>
        /// <remarks>Complicated query is <see cref="SearchPrefix"/>:<see cref="YandexSearchType"/>:limit:text</remarks>
        /// <param name="query">Target query</param>
        /// <param name="text">Search text</param>
        /// <param name="type">Search type</param>
        /// <param name="limit">Search limit</param>
        /// <returns>True if is this complicated query</returns>
        public bool TryParseQuery(string query, out string text, out YandexSearchType type, out int limit) {
            type = YandexSearchType.Track;
            limit = DefaultLimit;
            text = query;

            if (!query.StartsWith(SearchPrefix + ":")) return false;
            var match = SearchRegex.Match(query);
            if (!match.Success) return false;

            type = Enum.TryParse(typeof(YandexSearchType), match.Groups[2].Value, true, out var o) ? (YandexSearchType) o : YandexSearchType.Track;
            limit = int.TryParse(match.Groups[4].Value, out var i) ? i : DefaultLimit;
            text = match.Groups[5].Value;
            return true;
        }

        /// <summary>
        /// Perform search request on Yandex Music
        /// </summary>
        /// <param name="type">Search type</param>
        /// <param name="query">Search text</param>
        /// <param name="playlistLoader">Playlist loader instance</param>
        /// <param name="trackFactory">Track factory to create YandexMusicTrack from AudioTrackInfo</param>
        /// <param name="limit">Search results limit count</param>
        /// <returns>Instance of YandexMusicSearchResult</returns>
        /// <exception cref="Exception">Throws exception if something went wrong</exception>
        public async Task<YandexMusicSearchResult?> LoadSearchResult(YandexSearchType type, string query, YandexMusicPlaylistLoader playlistLoader,
                                                                     Func<AudioTrackInfo, YandexMusicTrack> trackFactory, int limit = DefaultLimit) {
            try {
                var searchResponse = await new YandexCustomRequest(_config)
                                          .Create(string.Format(TracksInfoFormat, type, query))
                                          .GetResponseAsync<MetaSearchResponse>();
                var albums = searchResponse.Albums?.Results.Take(limit);
                var playlists = searchResponse.Playlists?.Results.Take(limit);
                var tracks = searchResponse.Tracks?.Results.Take(limit);

                return new YandexMusicSearchResult(query, limit, type,
                    albums?.ToList().AsReadOnly(),
                    playlists?.ToList().AsReadOnly(),
                    tracks?.ToList().AsReadOnly());
            }
            catch (Exception e) {
                throw new Exception("Could not load search results", e);
            }
        }
    }
}