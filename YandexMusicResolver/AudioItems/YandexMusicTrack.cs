using System;
using System.Threading;
using System.Threading.Tasks;

namespace YandexMusicResolver.AudioItems {
    /// <summary>
    /// AudioTrackInfo wrapper to resolve track direct url
    /// </summary>
    public class YandexMusicTrack : IAudioItem {
        /// <summary>
        /// Get track info
        /// </summary>
        public AudioTrackInfo TrackInfo { get; }

        private YandexMusicMainResolver _mainResolver;
        private readonly Lazy<Task<string>> _directUrlLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicTrack"/> class.
        /// </summary>
        /// <param name="trackInfo">Track info</param>
        /// <param name="mainResolver">Resolver for direct url getting</param>
        public YandexMusicTrack(AudioTrackInfo trackInfo, YandexMusicMainResolver mainResolver) {
            _mainResolver = mainResolver;
            TrackInfo = trackInfo;
            _directUrlLoader = new Lazy<Task<string>>(GetDirectUrlInternal, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        /// <summary>
        /// Get direct url to track
        /// </summary>
        /// <remarks>If you not authorized will return 30s track version. This is YandexMusic restriction</remarks>
        /// <returns>Direct url to download track</returns>
        public Task<string> GetDirectUrl() {
            return _directUrlLoader.Value;
        }

        private async Task<string> GetDirectUrlInternal() {
            return await _mainResolver.DirectUrlLoader.GetDirectUrl(TrackInfo.Identifier, "mp3");
        }
    }
}