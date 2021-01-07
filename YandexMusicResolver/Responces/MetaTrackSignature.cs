using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responces {
    public class MetaTrackSignature : ITrackInfoContainer {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("albumId")]
        public long AlbumId { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }
        
        public async Task<AudioTrackInfo> ToAudioTrackInfo(YandexMusicTrackLoader loader) {
            return ((await loader.LoadTrack(AlbumId.ToString(), Id.ToString(), TrackFactory)) as YandexMusicTrack)?.TrackInfo!;
        }

        private YandexMusicTrack TrackFactory(AudioTrackInfo arg) {
            // This is fake for retrieving track info
            return new YandexMusicTrack(arg, null!);
        }
    }
}