using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Requests;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver.Loaders {
    public class YandexMusicTrackLoader {
        protected string? _token;

        public YandexMusicTrackLoader(string? token) {
            _token = token;
        }

        private const string TracksInfoFormat = "https://api.music.yandex.net/tracks?trackIds=";
        private const string TrackUrlFormat = "https://music.yandex.ru/album/{0}/track/{1}";
        public async Task<YandexMusicTrack?> LoadTrack(string albumId, string trackId, Func<AudioTrackInfo?, YandexMusicTrack?> trackFactory) {
            return trackFactory(await LoadTrackInfo(albumId, trackId));
        }

        public async Task<AudioTrackInfo?> LoadTrackInfo(string albumId, string trackId) {
            var response = await new YandexCustomRequest(_token).Create(TracksInfoFormat + $"{trackId}:{albumId}").GetResponseAsync<List<MetaTrack>>();
            var entry = response.First();
            return await entry.ToAudioTrackInfo(this);
        }
    }
}