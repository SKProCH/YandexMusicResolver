using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YandexMusicResolver.AudioItems;

namespace YandexMusicResolver.Tests;

public class MainResolverTest : YandexTestBase{
    [Theory]
    [InlineData("https://music.yandex.ru/album/9425747/track/55561798")]
    public async Task GetTrack(string url) {
        var audioItem = await MainResolver.ResolveQuery(url);
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
    public async Task GetAlbum(string url) {
        var audioItem = await MainResolver.ResolveQuery(url);
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
    public async Task GetPlaylist(string url) {
        var audioItem = await MainResolver.ResolveQuery(url);
        Assert.NotNull(audioItem);
        Assert.False(audioItem.IsSearchResult);
        Assert.NotNull(audioItem.Playlists);
        Assert.Null(audioItem.Tracks);
        Assert.Null(audioItem.Albums);
        Assert.NotEmpty(audioItem.Playlists);
        Assert.Equal(YandexSearchType.Playlist, audioItem.Type);
    }

    [Theory]
    [InlineData("Take over")]
    [InlineData("ymsearch:Track:10:Take over")]
    public async Task TestDisabledSearch(string query) {
        var yandexMusicMainResolver = YandexMusicMainResolver.Create(YandexCredentialsProviderMock.Object, new HttpClient());
        yandexMusicMainResolver.AllowSearch = false;
        var audioItem = await yandexMusicMainResolver.ResolveQuery(query);
        Assert.Null(audioItem);
    }

    [Theory]
    [InlineData("Take over", true)]
    [InlineData("ymsearch:Track:10:Take over", false)]
    public async Task TestDisabledPlainTextSearch(string query, bool isPlainText) {
        var yandexMusicMainResolver = YandexMusicMainResolver.Create(YandexCredentialsProviderMock.Object, new HttpClient());
        yandexMusicMainResolver.PlainTextIsSearchQuery = false;
        var audioItem = await yandexMusicMainResolver.ResolveQuery(query);
        Assert.Equal(isPlainText, audioItem == null);
    }

    [Theory]
    [InlineData("Take Over")]
    public async Task TestSearch(string url) {
        var audioItem = await MainResolver.ResolveQuery(url);
        Assert.NotNull(audioItem);
        Assert.IsType<YandexMusicSearchResult>(audioItem);
    }
}