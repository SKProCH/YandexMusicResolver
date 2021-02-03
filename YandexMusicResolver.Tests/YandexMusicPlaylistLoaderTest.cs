using Xunit;
using YandexMusicResolver.AudioItems;

namespace YandexMusicResolver.Tests {
    public class YandexMusicPlaylistLoaderTest : YandexTestBase {
        [Theory]
        [InlineData("9425747", "Renovatio", 12)]
        [InlineData("12033669", "Take Over", 1)]
        public void LoadAlbum(string albumId, string expectedName, int trackCount) {
            var album = MainResolver.PlaylistLoader.LoadAlbum(albumId).GetAwaiter().GetResult();
            Assert.NotNull(album);
            Assert.Equal(expectedName, album.Title);
            Assert.Equal(trackCount, album.Data.Count);
        }

        [Theory]
        [InlineData("enlivenbot", "1000", "Test1", 60)]
        [InlineData("enlivenbot", "1001", "Test2", 36)]
        public void LoadPlaylist(string userId, string playlistId, string expectedName, int trackCount) {
            var playlist = MainResolver.PlaylistLoader.LoadPlaylist(userId, playlistId).GetAwaiter().GetResult();
            Assert.NotNull(playlist);
            Assert.Equal(expectedName, playlist.Title);
            Assert.Equal(trackCount, playlist.Data.Count);
        }
    }
}