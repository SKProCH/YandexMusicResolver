using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responces {
    /// <summary>
    /// Represents data to resolve album
    /// </summary>
    public class MetaAlbumSignature {
        /// <summary>
        /// Id of this entity
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Title of this album
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Cover link
        /// </summary>
        [JsonProperty("coverUri")]
        public string? CoverUri { get; set; }

        /// <summary>
        /// Opengraph image (alternative cover) url
        /// </summary>
        [JsonProperty("ogImage")]
        public string? OgImage { get; set; }

        /// <summary>
        /// Count of tracks
        /// </summary>
        [JsonProperty("trackCount")]
        public long TrackCount { get; set; }

        /// <summary>
        /// Is this playlist available now
        /// </summary>
        [JsonProperty("available")]
        public bool Available { get; set; }

        /// <summary>
        /// Get full playlist with tracks
        /// </summary>
        /// <param name="yandexMusicPlaylistLoader">Instance of playlist loader to load playlist</param>
        /// <param name="trackFactory">Track factory to create YandexMusicTrack from AudioTrackInfo</param>
        /// <returns>Playlist with tracks</returns>
        public virtual async Task<YandexMusicPlaylist> GetPlaylist(YandexMusicPlaylistLoader yandexMusicPlaylistLoader,
                                                                   Func<AudioTrackInfo, YandexMusicTrack> trackFactory) {
            return (await yandexMusicPlaylistLoader.LoadPlaylist(Id.ToString(), trackFactory))!;
        }
    }
}