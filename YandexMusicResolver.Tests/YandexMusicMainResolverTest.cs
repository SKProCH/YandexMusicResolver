using Xunit;
using YandexMusicResolver.AudioItems;

namespace YandexMusicResolver.Tests {
    public class YandexMusicMainResolverTest : YandexTestBase{
        [Theory]
        [InlineData("https://music.yandex.ru/album/9425747/track/55561798")]
        public void GetTrack(string url) {
            var audioItem = MainResolver.ResolveQuery(url).GetAwaiter().GetResult();
            Assert.NotNull(audioItem);
            Assert.IsType<YandexMusicTrack>(audioItem);
        }
        
        [Theory]
        [InlineData("https://music.yandex.ru/album/9425747")]
        public void GetAlbum(string url) {
            var audioItem = MainResolver.ResolveQuery(url).GetAwaiter().GetResult();
            Assert.NotNull(audioItem);
            Assert.IsType<YandexMusicPlaylist>(audioItem);
        }
        
        [Theory]
        [InlineData("https://music.yandex.ru/users/enlivenbot/playlists/1000")]
        public void GetPlaylist(string url) {
            var audioItem = MainResolver.ResolveQuery(url).GetAwaiter().GetResult();
            Assert.NotNull(audioItem);
            Assert.IsType<YandexMusicPlaylist>(audioItem);
        }

        [Fact]
        public void TestDisabledSearch() {
            var yandexMusicMainResolver = new YandexMusicMainResolver(Config, false);
            var audioItem = yandexMusicMainResolver.ResolveQuery("Take Over").GetAwaiter().GetResult();
            Assert.Null(audioItem);
        }

        [Theory]
        [InlineData("Take Over")]
        public void TestSearch(string url) {
            var audioItem = MainResolver.ResolveQuery(url).GetAwaiter().GetResult();
            Assert.NotNull(audioItem);
            Assert.IsType<YandexMusicSearchResult>(audioItem);
        }
    }
}