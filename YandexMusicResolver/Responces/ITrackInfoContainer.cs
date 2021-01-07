using System.Threading.Tasks;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responces {
    public interface ITrackInfoContainer {
        Task<AudioTrackInfo> ToAudioTrackInfo(YandexMusicTrackLoader loader);
    }
}