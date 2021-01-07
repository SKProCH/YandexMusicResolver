using System;
using System.Threading;
using System.Threading.Tasks;

namespace YandexMusicResolver.AudioItems {
    public class YandexMusicTrack : IAudioItem {
        public AudioTrackInfo TrackInfo { get; }
        private YandexMusicMainResolver _mainResolver;
        private readonly Lazy<Task<string>> _directUrlLoader;
        
        public YandexMusicTrack(AudioTrackInfo trackInfo, YandexMusicMainResolver mainResolver) {
            _mainResolver = mainResolver;
            TrackInfo = trackInfo;
            _directUrlLoader = new Lazy<Task<string>>(GetDirectUrlInternal, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        public Task<string> GetDirectUrl() {
            return _directUrlLoader.Value;
        }
        
        private async Task<string> GetDirectUrlInternal() {
            return await _mainResolver.DirectUrlLoader.GetDirectUrl(TrackInfo.Identifier, "mp3");
        }
    }
}