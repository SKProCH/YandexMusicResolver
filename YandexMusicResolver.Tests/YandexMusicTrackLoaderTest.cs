﻿using Xunit;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Tests {
    public class YandexMusicTrackLoaderTest {
        [Theory]
        [InlineData("9425747", "55561798")]
        [InlineData("12033669", "70937156")]
        public void GetTracksInfo(string albumId, string trackId) {
            var yandexMusicTrackLoader = new YandexMusicTrackLoader(null);
            var trackInfo = yandexMusicTrackLoader.LoadTrackInfo(albumId, trackId).GetAwaiter().GetResult();
            Assert.NotNull(trackInfo);
            Assert.Equal($"https://music.yandex.ru/album/{albumId}/track/{trackId}", trackInfo.Uri);
        }
    }
}