using System.Threading.Tasks;
using Xunit;

namespace YandexMusicResolver.Tests;

public class PlaylistLoaderTest : YandexTestBase {
    [Theory]
    [InlineData("9425747", "Renovatio", 12)]
    [InlineData("35135225", "Atomic Heart, Vol.5", 23)]
    public async Task LoadAlbum(string albumId, string expectedName, int trackCount) {
        var album = await MainResolver.PlaylistLoader.LoadAlbum(albumId);
        Assert.NotNull(album);
        Assert.Equal(expectedName, album.Title);
        Assert.Equal(trackCount, album.Data.Count);
    }

    [Theory]
    [InlineData("enlivenbot", "1000")]
    [InlineData("universe0122", "1000")]
    [InlineData("olga--g", "1001")]
    [InlineData("ShadowKillerProPro", "1006")]
    public async Task LoadPlaylist(string userId, string playlistId) {
        var playlist = await MainResolver.PlaylistLoader.LoadPlaylist(userId, playlistId);
        Assert.NotNull(playlist);
        Assert.False(string.IsNullOrWhiteSpace(playlist.Title));
        Assert.True(playlist.Data.Count > 0);
    }
}