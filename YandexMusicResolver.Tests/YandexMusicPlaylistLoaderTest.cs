using System.Threading.Tasks;
using Xunit;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Tests {
    public class YandexMusicPlaylistLoaderTest {
        [Theory]
        [InlineData("9425747", "Renovatio", 12)]
        [InlineData("12033669", "Take Over", 1)] 
        public void LoadAlbum(string albumId, string expectedName, int trackCount) {
            var yandexMusicPlaylistLoader = new YandexMusicPlaylistLoader(null);
            var playlist = yandexMusicPlaylistLoader.LoadPlaylist(albumId,
                                                         info => new YandexMusicTrack(info, new YandexMusicMainResolver()))
                                                    .GetAwaiter().GetResult();
            Assert.NotNull(playlist);
            Assert.Equal(expectedName, playlist.Title);
            Assert.Equal(trackCount, playlist.Tracks.Count);
        }

        [Theory]
        [InlineData("enlivenbot", "1000", "Test1", 60)]
        [InlineData("enlivenbot", "1001", "Test2", 36)]
        public void LoadPlaylist(string userId, string playlistId, string expectedName, int trackCount) {
            var yandexMusicPlaylistLoader = new YandexMusicPlaylistLoader(null);
            var playlist = yandexMusicPlaylistLoader.LoadPlaylist(userId, playlistId,
                                                                    info => new YandexMusicTrack(info, new YandexMusicMainResolver()))
                                                               .GetAwaiter().GetResult();
            Assert.NotNull(playlist);
            Assert.Equal(expectedName, playlist.Title);
            Assert.Equal(trackCount, playlist.Tracks.Count);
        }
    }
}