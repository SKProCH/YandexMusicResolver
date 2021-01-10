#nullable enable
using System;
using System.Runtime.InteropServices.ComTypes;
using Xunit;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Tests {
    public class YandexMusicSearchResultLoaderTest {
        [Fact]
        public void DoTrackSearch() {
            var yandexMusicMainResolver = new YandexMusicMainResolver();
            var yandexMusicSearchResultLoader = new YandexMusicSearchResultLoader(null);
            Func<AudioTrackInfo, YandexMusicTrack> trackFactory = info => new YandexMusicTrack(info, yandexMusicMainResolver);
            var yandexMusicPlaylistLoader = new YandexMusicPlaylistLoader(null);

            var trackSearchResult = yandexMusicSearchResultLoader.LoadSearchResult(YandexSearchType.Track, "Take Over",
                yandexMusicPlaylistLoader, trackFactory).GetAwaiter().GetResult();
            Assert.NotNull(trackSearchResult);
            Assert.NotNull(trackSearchResult?.Tracks);
        }
        
        [Fact]
        public void DoAlbumSearch() {
            var yandexMusicMainResolver = new YandexMusicMainResolver();
            var yandexMusicSearchResultLoader = new YandexMusicSearchResultLoader(null);
            Func<AudioTrackInfo, YandexMusicTrack> trackFactory = info => new YandexMusicTrack(info, yandexMusicMainResolver);
            var yandexMusicPlaylistLoader = new YandexMusicPlaylistLoader(null);

            var trackSearchResult = yandexMusicSearchResultLoader.LoadSearchResult(YandexSearchType.Album, "Take Over",
                yandexMusicPlaylistLoader, trackFactory).GetAwaiter().GetResult();
            Assert.NotNull(trackSearchResult);
            Assert.NotNull(trackSearchResult?.Albums);
        }
        
        [Fact]
        public void DoAllSearch() {
            var yandexMusicMainResolver = new YandexMusicMainResolver();
            var yandexMusicSearchResultLoader = new YandexMusicSearchResultLoader(null);
            Func<AudioTrackInfo, YandexMusicTrack> trackFactory = info => new YandexMusicTrack(info, yandexMusicMainResolver);
            var yandexMusicPlaylistLoader = new YandexMusicPlaylistLoader(null);

            var trackSearchResult = yandexMusicSearchResultLoader.LoadSearchResult(YandexSearchType.All, "Take Over",
                yandexMusicPlaylistLoader, trackFactory).GetAwaiter().GetResult();
            Assert.NotNull(trackSearchResult);
            Assert.NotNull(trackSearchResult?.Albums);
            Assert.NotNull(trackSearchResult?.Playlists);
            Assert.NotNull(trackSearchResult?.Tracks);
        }
        
        [Fact]
        public void DoPlaylistSearch() {
            var yandexMusicMainResolver = new YandexMusicMainResolver();
            var yandexMusicSearchResultLoader = new YandexMusicSearchResultLoader(null);
            Func<AudioTrackInfo, YandexMusicTrack> trackFactory = info => new YandexMusicTrack(info, yandexMusicMainResolver);
            var yandexMusicPlaylistLoader = new YandexMusicPlaylistLoader(null);

            var trackSearchResult = yandexMusicSearchResultLoader.LoadSearchResult(YandexSearchType.Playlist, "Take Over",
                yandexMusicPlaylistLoader, trackFactory).GetAwaiter().GetResult();
            Assert.NotNull(trackSearchResult);
            Assert.NotNull(trackSearchResult?.Playlists);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("myprefix")]
        public void TestPrefixes(string? prefix) {
            var yandexMusicSearchResultLoader = new YandexMusicSearchResultLoader(null, prefix);
            prefix ??= "ymsearch";
            var correctQuery = $"{prefix}:playlist:25:take over";
            Assert.True(yandexMusicSearchResultLoader.TryParseQuery(correctQuery, out var text1, out var type1, out var limit1));
            Assert.Equal("take over", text1);
            Assert.Equal(YandexSearchType.Playlist, type1);
            Assert.Equal(25, limit1);

            var incorrectQuery = $"falseprefix:track:15:take over";
            Assert.False(yandexMusicSearchResultLoader.TryParseQuery(incorrectQuery, out var text2, out var type2, out var limit2));
            Assert.Equal("falseprefix:track:15:take over", text2);
            Assert.Equal(YandexSearchType.Track, type2);
            Assert.Equal(YandexMusicSearchResultLoader.DefaultLimit, limit2);

            var noQuery = "take over";
            Assert.False(yandexMusicSearchResultLoader.TryParseQuery(noQuery, out var text3, out var type3, out var limit3));
            Assert.Equal("take over", text3);
            Assert.Equal(YandexSearchType.Track, type3);
            Assert.Equal(YandexMusicSearchResultLoader.DefaultLimit, limit3);
        }
    }
}