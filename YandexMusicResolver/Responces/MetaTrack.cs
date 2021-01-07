using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YandexMusicResolver.Converters;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responces {
    public class MetaTrack : ITrackInfoContainer {
        private const string TrackUrlFormat = "https://music.yandex.ru/album/{0}/track/{1}";

        [JsonProperty("id")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; } = null!;

        [JsonProperty("available")]
        public bool Available { get; set; }

        [JsonProperty("durationMs")]
        public long DurationMs { get; set; }

        [JsonProperty("artists")]
        public MetaArtist[] Artists { get; set; } = null!;

        [JsonProperty("albums")]
        public MetaAlbumSignature[] Albums { get; set; } = null!;

        [JsonProperty("coverUri")]
        public string? CoverUri { get; set; }

        [JsonProperty("ogImage")]
        public string? OgImage { get; set; }

        [JsonProperty("lyricsAvailable")]
        public bool LyricsAvailable { get; set; }

        public Task<AudioTrackInfo> ToAudioTrackInfo() {
            var artists = string.Join(", ", Artists.Select(artist => artist.Name));
            var album = Albums.First();

            string? artworkUrl = null;
            TryApplyArtwork(ref artworkUrl, CoverUri);
            TryApplyArtwork(ref artworkUrl, OgImage);
            TryApplyArtwork(ref artworkUrl, album.CoverUri);
            TryApplyArtwork(ref artworkUrl, album.OgImage);

            return Task.FromResult(
                new AudioTrackInfo(
                    Title,
                    artists,
                    DurationMs,
                    Id.ToString(),
                    false,
                    string.Format(TrackUrlFormat, album.Id, Id),
                    new Dictionary<string, string> {{"artworkUrl", artworkUrl!}}));
        }

        public Task<AudioTrackInfo> ToAudioTrackInfo(YandexMusicTrackLoader loader) {
            return ToAudioTrackInfo();
        }

        private static void TryApplyArtwork(ref string? final, string artwork) {
            if (final != null) return;
            final = "https://" + artwork.Replace("%%", "200x200");
        }
    }
}