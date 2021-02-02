using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Converters;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responses {
    /// <summary>
    /// Track data from Yandex Music
    /// </summary>
    internal class MetaTrack {
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
        public List<YandexMusicArtist> Artists { get; set; } = null!;

        [JsonProperty("albums")]
        public List<MetaAlbumSignature> Albums { get; set; } = null!;

        [JsonProperty("coverUri")]
        public string? CoverUri { get; set; }

        [JsonProperty("ogImage")]
        public string? OgImage { get; set; }

        [JsonProperty("lyricsAvailable")]
        public bool LyricsAvailable { get; set; }

        public YandexMusicTrack ToYmTrack() {
            var album = Albums.First();

            string? artworkUrl = null;
            TryApplyArtwork(ref artworkUrl, CoverUri);
            TryApplyArtwork(ref artworkUrl, OgImage);
            TryApplyArtwork(ref artworkUrl, album.CoverUri);
            // TryApplyArtwork(ref artworkUrl, album.OgImage);

            return new YandexMusicTrack(Title, Artists, 
                TimeSpan.FromMilliseconds(DurationMs), Id.ToString(), 
                string.Format(TrackUrlFormat, album.Id, Id), artworkUrl);
        }

        private static void TryApplyArtwork(ref string? final, string? artwork) {
            if (final != null || artwork == null) return;
            final = "https://" + artwork.Replace("%%", "200x200");
        }
    }
}