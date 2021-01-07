using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver.Loaders {
    public class YandexMusicTrackLoader {
        private const string TracksInfoFormat = "https://api.music.yandex.net/tracks?trackIds=";
        private const string TrackUrlFormat = "https://music.yandex.ru/album/{0}/track/{1}";
        public async Task<YandexMusicTrack?> LoadTrack(string albumId, string trackId, Func<AudioTrackInfo?, YandexMusicTrack?> trackFactory) {
            return trackFactory(await LoadTrackInfo(albumId, trackId));
        }

        public async Task<AudioTrackInfo?> LoadTrackInfo(string albumId, string trackId) {
            var response = await WebRequestUtils.ExecuteGet(TracksInfoFormat + $"{trackId}:{albumId}").Parse<List<MetaTrack>>();
            var entry = response.First();
            return await entry.ToAudioTrackInfo(this);
        }
    }
}