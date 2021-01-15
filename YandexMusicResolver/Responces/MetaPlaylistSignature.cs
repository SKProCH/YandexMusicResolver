using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responces {
    /// <summary>
    /// Represents data to resolve playlist
    /// </summary>
    public class MetaPlaylistSignature : MetaAlbumSignature {
        [JsonProperty("uid")]
        [Obsolete]
        public long PlaylistId {
            set => Id = value;
        }

        /// <summary>
        /// Playlist owner info
        /// </summary>
        [JsonProperty("owner")]
        public MetaOwner Owner { get; set; } = null!;

        /// <inheritdoc />
        public override async Task<YandexMusicPlaylist> GetPlaylist(YandexMusicPlaylistLoader yandexMusicPlaylistLoader,
                                                                    Func<AudioTrackInfo, YandexMusicTrack> trackFactory) {
            return (await yandexMusicPlaylistLoader.LoadPlaylist(Owner.Uid.ToString(), Id.ToString(), trackFactory))!;
        }
    }
}