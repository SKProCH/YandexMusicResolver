using Xunit;
using YandexMusicResolver.AudioItems;

namespace YandexMusicResolver.Tests {
    public class YandexMusicMainResolverTest {
        [Theory]
        [InlineData("https://music.yandex.ru/album/9425747/track/55561798")]
        public void GetTrack(string url) {
            var yandexMusicMainResolver = new YandexMusicMainResolver();
            var audioItem = yandexMusicMainResolver.ResolveQuery(url).GetAwaiter().GetResult();
            Assert.NotNull(audioItem);
            Assert.IsType<YandexMusicTrack>(audioItem);
        }
        
        [Theory]
        [InlineData("https://music.yandex.ru/album/9425747")]
        public void GetAlbum(string url) {
            var yandexMusicMainResolver = new YandexMusicMainResolver();
            var audioItem = yandexMusicMainResolver.ResolveQuery(url).GetAwaiter().GetResult();
            Assert.NotNull(audioItem);
            Assert.IsType<YandexMusicPlaylist>(audioItem);
        }
        
        [Theory]
        [InlineData("https://music.yandex.ru/users/enlivenbot/playlists/1000")]
        public void GetPlaylist(string url) {
            var yandexMusicMainResolver = new YandexMusicMainResolver();
            var audioItem = yandexMusicMainResolver.ResolveQuery(url).GetAwaiter().GetResult();
            Assert.NotNull(audioItem);
            Assert.IsType<YandexMusicPlaylist>(audioItem);
        }

        [Fact]
        public void TestDisabledSearch() {
            var yandexMusicMainResolver = new YandexMusicMainResolver(false);
            var audioItem = yandexMusicMainResolver.ResolveQuery("Take Over").GetAwaiter().GetResult();
            Assert.Null(audioItem);
        }

        [Theory]
        [InlineData("Take Over")]
        public void TestSearch(string url) {
            var yandexMusicMainResolver = new YandexMusicMainResolver();
            var audioItem = yandexMusicMainResolver.ResolveQuery(url).GetAwaiter().GetResult();
            Assert.NotNull(audioItem);
            Assert.IsType<YandexMusicSearchResult>(audioItem);
        }
    }
}