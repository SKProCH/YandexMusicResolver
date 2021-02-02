using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responses {
    internal class MetaPlaylistSignature {
        [JsonProperty("uid")]
        public long Uid { get; set; }

        [JsonProperty("kind")]
        public long Kind { get; set; }

        [JsonProperty("trackCount")]
        public long TrackCount { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("owner")]
        public YandexMusicOwner Owner { get; set; }

        [JsonProperty("cover")]
        public MetaCover Cover { get; set; }

        public virtual YandexMusicPlaylist ToYaPlaylist(YandexMusicPlaylistLoader loader) {
            return new(Uid, Kind, TrackCount, Title, Owner, Cover.GetCoverUrl(), loader);
        }
    }
}