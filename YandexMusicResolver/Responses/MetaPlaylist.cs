using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responses {
    internal class MetaPlaylist : MetaPlaylistSignature {
        [JsonProperty("tracks")]
        private List<MetaPlaylistTrackContainer> PlaylistTracks {
            set => Tracks = value.ToDictionary(pair => pair.Id.ToString(), pair => pair.Track?.ToYmTrack());
        }
        
        public Dictionary<string, YandexMusicTrack?> Tracks { get; set; } = null!;
        public override YandexMusicPlaylist ToYaPlaylist(IYandexMusicPlaylistLoader yandexMusicPlaylistLoader) {
            #pragma warning disable 8620
            return new(Uid, Kind, TrackCount, Title, Owner, Cover.GetCoverUrl(), Tracks.Values.Where(track => track != null).ToList());
            #pragma warning restore 8620
        }
    }
}