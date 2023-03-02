using Xunit;

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
        [InlineData("enlivenbot", "1000")]
        [InlineData("universe0122", "1000")]
        [InlineData("olga--g", "1001")]
        [InlineData("ShadowKillerProPro", "1006")]
        public void LoadPlaylist(string userId, string playlistId) {
            var playlist = MainResolver.PlaylistLoader.LoadPlaylist(userId, playlistId).GetAwaiter().GetResult();
            Assert.NotNull(playlist);
            Assert.False(string.IsNullOrWhiteSpace(playlist.Title));
            Assert.True(playlist.Data.Count > 0);
        }
    }
}