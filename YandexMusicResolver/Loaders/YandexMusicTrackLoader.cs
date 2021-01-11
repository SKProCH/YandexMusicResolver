using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Config;
using YandexMusicResolver.Requests;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver.Loaders {
    public class YandexMusicTrackLoader {
        protected readonly IYandexConfig Config;

        public YandexMusicTrackLoader(IYandexConfig config) {
            Config = config;
        }

        private const string TracksInfoFormat = "https://api.music.yandex.net/tracks?trackIds=";

        public async Task<YandexMusicTrack?> LoadTrack(string albumId, string trackId, Func<AudioTrackInfo, YandexMusicTrack?> trackFactory) {
            return trackFactory((await LoadTrackInfo(albumId, trackId))!);
        }

        public async Task<AudioTrackInfo?> LoadTrackInfo(string albumId, string trackId) {
            var response = await new YandexCustomRequest(Config).Create(TracksInfoFormat + $"{trackId}:{albumId}").GetResponseAsync<List<MetaTrack>>();
            var entry = response.First();
            return await entry.ToAudioTrackInfo(this);
        }
    }
}