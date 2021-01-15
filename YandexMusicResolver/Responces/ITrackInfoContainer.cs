using System.Threading.Tasks;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responces {
    /// <summary>
    /// Represents entity from which we can get <see cref="AudioTrackInfo"/>
    /// </summary>
    public interface ITrackInfoContainer {
        /// <summary>
        /// Get related <see cref="AudioTrackInfo"/>
        /// </summary>
        /// <param name="loader">Track loader instance</param>
        /// <returns>Instance of <see cref="AudioTrackInfo"/></returns>
        Task<AudioTrackInfo> ToAudioTrackInfo(YandexMusicTrackLoader loader);
    }
}