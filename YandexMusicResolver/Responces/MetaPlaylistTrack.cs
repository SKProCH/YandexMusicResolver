using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responces {
    /// <summary>
    /// Represent
    /// </summary>
    internal class MetaPlaylistTrack : ITrackInfoContainer {
        /// <summary>
        /// Track id
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// List of albums that contain this track
        /// </summary>
        [JsonProperty("albums")]
        public List<MetaAlbumSignature> Albums { get; set; } = null!;

        /// <summary>
        /// Creation timestamp
        /// </summary>
        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        /// <inheritdoc />
        public async Task<AudioTrackInfo> ToAudioTrackInfo(YandexMusicTrackLoader loader) {
            return (await loader.LoadTrack(Id.ToString(), TrackFactory))?.TrackInfo!;
        }

        private YandexMusicTrack TrackFactory(AudioTrackInfo arg) {
            // This is fake for retrieving track info
            return new YandexMusicTrack(arg, null!);
        }
    }
}