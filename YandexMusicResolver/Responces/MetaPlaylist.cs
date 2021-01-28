using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace YandexMusicResolver.Responces {
    internal class MetaPlaylist {
        [JsonProperty("title")]
        public string Title { get; set; } = null!;

        [JsonProperty("trackCount")]
        public long TrackCount { get; set; }

        [JsonProperty("ogImage")]
        public string? OgImage { get; set; }

        [JsonProperty("coverUri")]
        public string? CoverUri { get; set; }

        [JsonProperty("available")]
        public bool Available { get; set; } = true;

        [JsonProperty("tracks")]
        private List<MetaPlaylistTrackContainer> PlaylistTracks {
            set => Tracks = value.Select(container => container.Track).Cast<ITrackInfoContainer>().ToList();
        }

        [JsonProperty("volumes")]
        private List<List<MetaTrack>> AlbumVolumes {
            set => Tracks = value.SelectMany(list => list).Cast<ITrackInfoContainer>().ToList();
        }

        public List<ITrackInfoContainer> Tracks { get; set; } = null!;
    }
}