using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Config;
using YandexMusicResolver.Ids;
using YandexMusicResolver.Responses;

namespace YandexMusicResolver.Loaders;

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
    public YandexMusicTrackLoader(IYandexCredentialsProvider credentialsProvider, IHttpClientFactory httpClientFactory)
        : this(credentialsProvider, httpClientFactory.GetYMusicHttpClient()) { }

    private YandexMusicTrackLoader(IYandexCredentialsProvider credentialsProvider, HttpClient httpClient) {
        _credentialsProvider = credentialsProvider;
        _httpClient = httpClient;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="YandexMusicTrackLoader"/> class.
    /// </summary>
    /// <param name="credentialsProvider">Config instance for performing requests</param>
    /// <param name="httpClient">HttpClient for performing requests. But preferred way is use another ctor and pass <see cref="IHttpClientFactory"/></param>
    public static YandexMusicTrackLoader CreateWithHttpClient(IYandexCredentialsProvider credentialsProvider, HttpClient httpClient) {
        return new YandexMusicTrackLoader(credentialsProvider, httpClient);
    }

    /// <inheritdoc />
    public Task<YandexMusicTrack?> LoadTrack(long trackId) => LoadTrack(trackId.ToString());

    /// <inheritdoc />
    public Task<YandexMusicTrack?> LoadTrack(Guid trackId) => LoadTrack(trackId.ToString());

    /// <inheritdoc />
    public Task<YandexMusicTrack?> LoadTrack(YandexId trackId) => LoadTrack(trackId.ToString());

    /// <inheritdoc />
    public async Task<YandexMusicTrack?> LoadTrack(string trackId) {
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
    public Task<IReadOnlyCollection<YandexMusicTrack>> LoadTracks(IEnumerable<long> trackIds) => LoadTracks(trackIds.Select(l => l.ToString()));

    /// <inheritdoc />
    public Task<IReadOnlyCollection<YandexMusicTrack>> LoadTracks(IEnumerable<Guid> trackIds) => LoadTracks(trackIds.Select(l => l.ToString()));

    /// <inheritdoc />
    public Task<IReadOnlyCollection<YandexMusicTrack>> LoadTracks(IEnumerable<YandexId> trackIds) => LoadTracks(trackIds.Select(l => l.ToString()));

    /// <inheritdoc />
    public async Task<IReadOnlyCollection<YandexMusicTrack>> LoadTracks(IEnumerable<string> trackIds) {
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