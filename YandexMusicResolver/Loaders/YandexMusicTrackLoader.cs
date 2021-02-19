using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Config;
using YandexMusicResolver.Requests;
using YandexMusicResolver.Responses;

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
            config.Load();
            Config = config;
        }

        private const string TracksInfoFormat = "https://api.music.yandex.net/tracks?trackIds=";

        /// <summary>
        /// Load track info
        /// </summary>
        /// <param name="trackId">Target track id</param>
        /// <returns>Instance of <see cref="YandexMusicTrack"/></returns>
        public async Task<YandexMusicTrack?> LoadTrack(string trackId) {
            var response = await new YandexCustomRequest(Config).Create(TracksInfoFormat + trackId).GetResponseAsync<List<MetaTrack>>();
            return response.FirstOrDefault()?.ToYmTrack();
        }
        
        /// <summary>
        /// Load track infos
        /// </summary>
        /// <param name="trackIds">Target track ids</param>
        /// <returns>List of instances of <see cref="YandexMusicTrack"/></returns>
        public async Task<List<YandexMusicTrack>> LoadTracks(IEnumerable<string> trackIds) {
            var response = await new YandexCustomRequest(Config).Create(TracksInfoFormat + string.Join(',', trackIds)).GetResponseAsync<List<MetaTrack>>();
            return response.Select(track => track.ToYmTrack()).ToList();
        }
    }
}