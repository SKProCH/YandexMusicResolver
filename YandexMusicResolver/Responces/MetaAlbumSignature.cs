using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responces {
    public class MetaAlbumSignature
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("coverUri")]
        public string? CoverUri { get; set; }

        [JsonProperty("ogImage")]
        public string? OgImage { get; set; }

        [JsonProperty("trackCount")]
        public long TrackCount { get; set; }

        [JsonProperty("available")]
        public bool Available { get; set; }
        
        public virtual async Task<YandexMusicPlaylist> GetPlaylist(YandexMusicPlaylistLoader yandexMusicPlaylistLoader, Func<AudioTrackInfo, YandexMusicTrack> trackFactory) {
            return (await yandexMusicPlaylistLoader.LoadPlaylist(Id.ToString(), trackFactory))!;
        }
    }
}