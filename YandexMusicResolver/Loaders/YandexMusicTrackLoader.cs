using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Config;
using YandexMusicResolver.Requests;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver.Loaders {
    /// <summary>
    /// Represents track info loader from Yandex Music
    /// </summary>
    public class YandexMusicTrackLoader {
        /// <summary>
        /// Config instance for performing requests
        /// </summary>
        protected readonly IYandexConfig Config;

        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicTrackLoader"/> class.
        /// </summary>
        /// <param name="config">Config instance for performing requests</param>
        public YandexMusicTrackLoader(IYandexConfig config) {
            Config = config;
        }

        private const string TracksInfoFormat = "https://api.music.yandex.net/tracks?trackIds=";

        /// <summary>
        /// Load track
        /// </summary>
        /// <param name="trackId">Target track id</param>
        /// <param name="trackFactory">Track factory to create YandexMusicTrack from AudioTrackInfo</param>
        /// <returns>Instance of <see cref="YandexMusicTrack"/></returns>
        public async Task<YandexMusicTrack?> LoadTrack(string trackId, Func<AudioTrackInfo, YandexMusicTrack?> trackFactory) {
            return trackFactory((await LoadTrackInfo(trackId))!);
        }
        
        /// <summary>
        /// Load track info
        /// </summary>
        /// <param name="trackId">Target track id</param>
        /// <returns>Instance of <see cref="AudioTrackInfo"/></returns>
        public async Task<AudioTrackInfo?> LoadTrackInfo(string trackId) {
            var response = await new YandexCustomRequest(Config).Create(TracksInfoFormat + trackId).GetResponseAsync<List<MetaTrack>>();
            var entry = response.First();
            return await entry.ToAudioTrackInfo(this);
        }
    }
}