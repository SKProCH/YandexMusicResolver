using Xunit;
using YandexMusicResolver.AudioItems;

namespace YandexMusicResolver.Tests {
    public class YandexMusicMainResolverTest : YandexTestBase{
        [Theory]
        [InlineData("https://music.yandex.ru/album/9425747/track/55561798")]
        public void GetTrack(string url) {
            var audioItem = MainResolver.ResolveQuery(url).GetAwaiter().GetResult();
            Assert.NotNull(audioItem);
            Assert.False(audioItem.IsSearchResult);
            Assert.NotNull(audioItem.Tracks);
            Assert.Null(audioItem.Playlists);
            Assert.Null(audioItem.Albums);
            Assert.NotEmpty(audioItem.Tracks);
            Assert.Equal(YandexSearchType.Track, YandexSearchType.Track);
        }
        
        [Theory]
        [InlineData("https://music.yandex.ru/album/9425747")]
        public void GetAlbum(string url) {
            var audioItem = MainResolver.ResolveQuery(url).GetAwaiter().GetResult();
            Assert.NotNull(audioItem);
            Assert.False(audioItem.IsSearchResult);
            Assert.NotNull(audioItem.Albums);
            Assert.Null(audioItem.Tracks);
            Assert.Null(audioItem.Playlists);
            Assert.NotEmpty(audioItem.Albums);
            Assert.Equal(YandexSearchType.Album, audioItem.Type);
        }
        
        [Theory]
        [InlineData("https://music.yandex.ru/users/enlivenbot/playlists/1000")]
        public void GetPlaylist(string url) {
            var audioItem = MainResolver.ResolveQuery(url).GetAwaiter().GetResult();
            Assert.NotNull(audioItem);
            Assert.False(audioItem.IsSearchResult);
            Assert.NotNull(audioItem.Playlists);
            Assert.Null(audioItem.Tracks);
            Assert.Null(audioItem.Albums);
            Assert.NotEmpty(audioItem.Playlists);
            Assert.Equal(YandexSearchType.Playlist, audioItem.Type);
        }

        [Fact]
        public void TestDisabledSearch() {
            var yandexMusicMainResolver = new YandexMusicMainResolver(Config) {AllowSearch = false};
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