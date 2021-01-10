using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Requests;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver.Loaders {
    public class YandexMusicSearchResultLoader {
        public const int DefaultLimit = 10;
        private const string TracksInfoFormat = "https://api.music.yandex.net/search?type={0}&page=0&text={1}";
        private Regex SearchRegex;
        private string? _token;

        public YandexMusicSearchResultLoader(string? token, string? searchPrefix = null) {
            // ReSharper disable once StringLiteralTypo
            searchPrefix ??= "ymsearch";
            _token = token;
            SearchRegex = new Regex($"{searchPrefix}(:([a-zA-Z]+))?(:([0-9]+))?:([^:]+)");
        }

        public Task<YandexMusicSearchResult?> LoadSearchResult(string query, YandexMusicPlaylistLoader playlistLoader,
                                                               Func<AudioTrackInfo, YandexMusicTrack> trackFactory) {
            TryParseQuery(query, out var text, out var type, out var limit);
            return LoadSearchResult(type, text, playlistLoader, trackFactory, limit);
        }

        public bool TryParseQuery(string query, out string text, out YandexSearchType type, out int limit) {
            type = YandexSearchType.Track;
            limit = DefaultLimit;
            text = query;

            var match = SearchRegex.Match(query);
            if (!match.Success) return false;

            type = Enum.TryParse(typeof(YandexSearchType), match.Groups[2].Value, true, out var o) ? (YandexSearchType) o : YandexSearchType.Track;
            limit = int.TryParse(match.Groups[4].Value, out var i) ? i : DefaultLimit;
            text = match.Groups[5].Value;
            return true;
        }

        public async Task<YandexMusicSearchResult?> LoadSearchResult(YandexSearchType type, string query, YandexMusicPlaylistLoader playlistLoader,
                                                                     Func<AudioTrackInfo, YandexMusicTrack> trackFactory, int limit = DefaultLimit) {
            try {
                var searchResponse = await new YandexCustomRequest(_token)
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