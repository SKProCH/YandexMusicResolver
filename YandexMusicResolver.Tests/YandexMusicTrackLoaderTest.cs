using System.Collections.Generic;
using Xunit;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Tests {
    public class YandexMusicTrackLoaderTest : YandexTestBase {
        [Theory]
        [InlineData(9425747, 55561798)]
        [InlineData(12033669, 70937156)]
        public void GetTrackInfo(long albumId, long trackId) {
            var yandexMusicTrackLoader = new YandexMusicTrackLoader(Config);
            var trackInfo = yandexMusicTrackLoader.LoadTrack(trackId).GetAwaiter().GetResult();
            Assert.NotNull(trackInfo);
            Assert.Equal($"https://music.yandex.ru/album/{albumId}/track/{trackId}", trackInfo.Uri);
        }

        [Fact]
        public void GetTracksInfo() {
            var yandexMusicTrackLoader = new YandexMusicTrackLoader(Config);
            var trackIds = new List<long>() { 55561798, 70937156 };
            var trackInfo = yandexMusicTrackLoader.LoadTracks(trackIds).GetAwaiter().GetResult();
            Assert.NotNull(trackInfo);
            Assert.Equal(trackIds.Count, trackInfo.Count);
        }
    }
}