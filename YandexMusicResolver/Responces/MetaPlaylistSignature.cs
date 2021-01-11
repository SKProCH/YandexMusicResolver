using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responces {
    public class MetaPlaylistSignature : MetaAlbumSignature {
        [JsonProperty("uid")]
        [Obsolete]
        public long PlaylistId {
            set => Id = value;
        }

        [JsonProperty("owner")]
        public MetaOwner Owner { get; set; } = null!;
        
        public override async Task<YandexMusicPlaylist> GetPlaylist(YandexMusicPlaylistLoader yandexMusicPlaylistLoader, Func<AudioTrackInfo, YandexMusicTrack> trackFactory) {
            return (await yandexMusicPlaylistLoader.LoadPlaylist(Owner.Uid.ToString(), Id.ToString(), trackFactory))!;
        }
    }
}