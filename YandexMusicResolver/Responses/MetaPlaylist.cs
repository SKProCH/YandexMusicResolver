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
            set => Tracks = value.Select(container => container.Track).ToList();
        }
        
        public List<MetaTrack> Tracks { get; set; } = null!;
        public override YandexMusicPlaylist ToYaPlaylist(YandexMusicPlaylistLoader loader) {
            return new(Uid, Kind, TrackCount, Title, Owner, Cover.GetCoverUrl(), Tracks.Select(track => track.ToYmTrack()).ToList());
        }
    }
}