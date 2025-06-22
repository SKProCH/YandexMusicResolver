using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YandexMusicResolver.AudioItems;

namespace YandexMusicResolver.Tests;

public class MainResolverTest : YandexTestBase
{
    [Theory]
    [InlineData("https://music.yandex.ru/album/9425747/track/55561798")]
    public async Task GetTrack(string url)
    {
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
    public async Task GetAlbum(string url)
    {
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
    public async Task GetPlaylist(string url)
    {
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
    public async Task TestDisabledSearch(string query)
    {
        var yandexMusicMainResolver =
            YandexMusicMainResolver.Create(YandexCredentialsProviderMock.Object, new HttpClient());
        yandexMusicMainResolver.AllowSearch = false;
        var audioItem = await yandexMusicMainResolver.ResolveQuery(query);
        Assert.Null(audioItem);
    }

    [Theory]
    [InlineData("Take over", true)]
    [InlineData("ymsearch:Track:10:Take over", false)]
    public async Task TestDisabledPlainTextSearch(string query, bool isPlainText)
    {
        var yandexMusicMainResolver =
            YandexMusicMainResolver.Create(YandexCredentialsProviderMock.Object, new HttpClient());
        yandexMusicMainResolver.PlainTextIsSearchQuery = false;
        var audioItem = await yandexMusicMainResolver.ResolveQuery(query);
        Assert.Equal(isPlainText, audioItem == null);
    }

    [Theory]
    [InlineData("Take Over")]
    public async Task TestSearch(string url)
    {
        MainResolver.PlainTextIsSearchQuery = true;
        var audioItem = await MainResolver.ResolveQuery(url);
        Assert.NotNull(audioItem);
        Assert.IsType<YandexMusicSearchResult>(audioItem);
    }

    [Theory]
    [InlineData("https://music.yandex.ru/album/9425747/track/55561798")]
    [InlineData("https://music.yandex.ru/album/9425747")]
    [InlineData("https://music.yandex.ru/users/enlivenbot/playlists/1000")]
    [InlineData("ymsearch:Track:10:Take over")]
    [InlineData("https://music.yandex.ru/album/36938610/track/139888583?utm_source=desktop&utm_medium=copy_link")]
    public void CanResolveQuery(string url)
    {
        Assert.True(MainResolver.CanResolveQuery(url));
    }

    [Theory]
    [InlineData("some text")]
    public void CantResolveQuery(string url)
    {
        Assert.False(MainResolver.CanResolveQuery(url));
    }
}