using Xunit;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Tests {
    public class YandexMusicTrackLoaderTest : YandexTestBase {
        [Theory]
        [InlineData("9425747", "55561798")]
        [InlineData("12033669", "70937156")]
        public void GetTracksInfo(string albumId, string trackId) {
            var yandexMusicTrackLoader = new YandexMusicTrackLoader(Config);
            var trackInfo = yandexMusicTrackLoader.LoadTrackInfo(trackId).GetAwaiter().GetResult();
            Assert.NotNull(trackInfo);
            Assert.Equal($"https://music.yandex.ru/album/{albumId}/track/{trackId}", trackInfo.Uri);
        }
    }
}