using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Config;
using YandexMusicResolver.Requests;
using YandexMusicResolver.Responses;

namespace YandexMusicResolver.Loaders {
    /// <inheritdoc />
    public class YandexMusicTrackLoader : IYandexMusicTrackLoader {
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

        /// <inheritdoc />
        public async Task<YandexMusicTrack?> LoadTrack(string trackId) {
            try {
                var response = await new YandexCustomRequest(Config).Create(TracksInfoFormat + trackId).GetResponseAsync<List<MetaTrack>>();
                return response.FirstOrDefault()?.ToYmTrack();
            }
            catch (Exception e) {
                throw new YandexMusicException("Exception while loading track", e);
            }
        }

        /// <inheritdoc />
        public async Task<List<YandexMusicTrack>> LoadTracks(IEnumerable<string> trackIds) {
            try {
                var response = await new YandexCustomRequest(Config).Create(TracksInfoFormat + string.Join(',', trackIds)).GetResponseAsync<List<MetaTrack>>();
                return response.Select(track => track.ToYmTrack()).ToList();
            }
            catch (Exception e) {
                throw new YandexMusicException("Exception while loading tracks", e);
            }
        }
    }
}