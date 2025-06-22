using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Tests;

public class TrackLoaderTest : YandexTestBase {
    [Theory]
    [InlineData(9425747, 55561798)]
    [InlineData(35135225, 135525714)]
    public async Task GetTrackInfo(long albumId, long trackId) {
        var yandexMusicTrackLoader = YandexMusicTrackLoader.CreateWithHttpClient(YandexCredentialsProviderMock.Object, new HttpClient());
        var trackInfo = await yandexMusicTrackLoader.LoadTrack(trackId);
        Assert.NotNull(trackInfo);
        Assert.Equal($"https://music.yandex.ru/album/{albumId}/track/{trackId}", trackInfo.Uri);
    }

    [Fact]
    public async Task GetTracksInfo() {
        var yandexMusicTrackLoader = YandexMusicTrackLoader.CreateWithHttpClient(YandexCredentialsProviderMock.Object, new HttpClient());
        var trackIds = new List<long>() { 55561798, 70937156 };
        var trackInfo = await yandexMusicTrackLoader.LoadTracks(trackIds);
        Assert.NotNull(trackInfo);
        Assert.Equal(trackIds.Count, trackInfo.Count);
    }
}