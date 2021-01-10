using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responces {
    public class MetaPlaylistTrack : ITrackInfoContainer {
        [JsonProperty("id")]
        public long Id { get; set; }
        
        [JsonProperty("albums")]
        public List<MetaAlbumSignature> Albums { get; set; } = null!;

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }
        
        public async Task<AudioTrackInfo> ToAudioTrackInfo(YandexMusicTrackLoader loader) {
            return (await loader.LoadTrack(Albums.First().Id.ToString(), Id.ToString(), TrackFactory))?.TrackInfo!;
        }

        private YandexMusicTrack TrackFactory(AudioTrackInfo arg) {
            // This is fake for retrieving track info
            return new YandexMusicTrack(arg, null!);
        }
    }
}