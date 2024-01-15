using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Ids;

namespace YandexMusicResolver.Loaders {
    /// <summary>
    /// Represents track info loader from Yandex Music
    /// </summary>
    public interface IYandexMusicTrackLoader {
        /// <summary>
        /// Load track info
        /// </summary>
        /// <param name="trackId">Target track id</param>
        /// <returns>Instance of <see cref="YandexMusicTrack"/></returns>
        Task<YandexMusicTrack?> LoadTrack(long trackId);
        
        /// <summary>
        /// Load track info
        /// </summary>
        /// <param name="trackId">Target track id</param>
        /// <returns>Instance of <see cref="YandexMusicTrack"/></returns>
        Task<YandexMusicTrack?> LoadTrack(Guid trackId);
        
        /// <summary>
        /// Load track info
        /// </summary>
        /// <param name="trackId">Target track id</param>
        /// <returns>Instance of <see cref="YandexMusicTrack"/></returns>
        Task<YandexMusicTrack?> LoadTrack(YandexId trackId);
        
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
        Task<IReadOnlyCollection<YandexMusicTrack>> LoadTracks(IEnumerable<long> trackIds);
        
        /// <summary>
        /// Load track infos
        /// </summary>
        /// <param name="trackIds">Target track ids</param>
        /// <returns>List of instances of <see cref="YandexMusicTrack"/></returns>
        Task<IReadOnlyCollection<YandexMusicTrack>> LoadTracks(IEnumerable<Guid> trackIds);
        
        /// <summary>
        /// Load track infos
        /// </summary>
        /// <param name="trackIds">Target track ids</param>
        /// <returns>List of instances of <see cref="YandexMusicTrack"/></returns>
        Task<IReadOnlyCollection<YandexMusicTrack>> LoadTracks(IEnumerable<YandexId> trackIds);
        
        /// <summary>
        /// Load track infos
        /// </summary>
        /// <param name="trackIds">Target track ids</param>
        /// <returns>List of instances of <see cref="YandexMusicTrack"/></returns>
        Task<IReadOnlyCollection<YandexMusicTrack>> LoadTracks(IEnumerable<string> trackIds);
    }
}
