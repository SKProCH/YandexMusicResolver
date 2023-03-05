using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Config;
using YandexMusicResolver.Responses;

namespace YandexMusicResolver.Loaders {
    /// <inheritdoc />
    public class YandexMusicTrackLoader : IYandexMusicTrackLoader {
        private const string TracksInfoFormat = "https://api.music.yandex.net/tracks?trackIds=";
        /// <summary>
        /// Config instance for performing requests
        /// </summary>
        private readonly IYandexCredentialsProvider _credentialsProvider;
        private HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicTrackLoader"/> class.
        /// </summary>
        /// <param name="credentialsProvider">Config instance for performing requests</param>
        /// <param name="httpClientFactory">Factory for resolving HttpClient. Client name is <see cref="YandexMusicUtilities.HttpClientName"/></param>
        public YandexMusicTrackLoader(IYandexCredentialsProvider credentialsProvider, IHttpClientFactory httpClientFactory) {
            _credentialsProvider = credentialsProvider;
            _httpClient = httpClientFactory.GetYMusicHttpClient();
        }        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicTrackLoader"/> class.
        /// </summary>
        /// <param name="credentialsProvider">Config instance for performing requests</param>
        /// <param name="httpClient">HttpClient for performing requests. But preferred way is use another ctor and pass <see cref="IHttpClientFactory"/></param>
        public YandexMusicTrackLoader(IYandexCredentialsProvider credentialsProvider, HttpClient httpClient) {
            _credentialsProvider = credentialsProvider;
            _httpClient = httpClient;
        }

        /// <inheritdoc />
        public async Task<YandexMusicTrack?> LoadTrack(long trackId) {
            try {
                var url = TracksInfoFormat + trackId;
                var response = await _httpClient.PerformYMusicRequestAsync<List<MetaTrack>>(_credentialsProvider, url);
                return response.FirstOrDefault()?.ToYmTrack();
            }
            catch (Exception e) {
                throw new YandexMusicException("Exception while loading track", e);
            }
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<YandexMusicTrack>> LoadTracks(IEnumerable<long> trackIds) {
            try {
                var trackIdsString = string.Join(",", trackIds);
                var url = TracksInfoFormat + trackIdsString;
                var response = await _httpClient.PerformYMusicRequestAsync<List<MetaTrack>>(_credentialsProvider, url);
                return response.Select(track => track.ToYmTrack()).ToList();
            }
            catch (Exception e) {
                throw new YandexMusicException("Exception while loading tracks", e);
            }
        }
    }
}
