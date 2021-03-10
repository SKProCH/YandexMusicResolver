using System.Collections.Generic;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;

namespace YandexMusicResolver.Loaders {
    public interface IYandexMusicTrackLoader {
        /// <summary>
        /// Load track info
        /// </summary>
        /// <param name="trackId">Target track id</param>
        /// <returns>Instance of <see cref="YandexMusicTrack"/></returns>
        Task<YandexMusicTrack?> LoadTrack(string trackId);

        /// <summary>
        /// Load track infos
        /// </summary>
        /// <param name="trackIds">Target track ids</param>
        /// <returns>List of instances of <see cref="YandexMusicTrack"/></returns>
        Task<List<YandexMusicTrack>> LoadTracks(IEnumerable<string> trackIds);
    }
}