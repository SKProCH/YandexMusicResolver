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
    
    [Theory]
    [InlineData("lk.aa3d0094-6960-405c-bcc0-60b3c3c8cf5e")]
    [InlineData("lk.e82a550e-63f9-4c8d-8ed0-ae15056051d8")]
    public async Task LoadPlaylistByGuid(string playlistGuid) {
        var playlist = await MainResolver.PlaylistLoader.LoadPlaylist(playlistGuid);
        Assert.NotNull(playlist);
        Assert.False(string.IsNullOrWhiteSpace(playlist.Title));
        Assert.True(playlist.Data.Count > 0);
    }
}