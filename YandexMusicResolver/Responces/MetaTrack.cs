using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YandexMusicResolver.Converters;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responces {
    /// <summary>
    /// Track data from Yandex Music
    /// </summary>
    public class MetaTrack : ITrackInfoContainer {
        private const string TrackUrlFormat = "https://music.yandex.ru/album/{0}/track/{1}";

        /// <summary>
        /// Track ID
        /// </summary>
        [JsonProperty("id")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Id { get; set; }

        /// <summary>
        /// Track title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; } = null!;

        /// <summary>
        /// Is track available
        /// </summary>
        /// <remarks>If false, then most likely you are trying to get this information while not in the CIS. At the moment, you need to use a proxy in the CIS or log in with an account that has Yandex Plus (a subscription from Yandex)</remarks>
        [JsonProperty("available")]
        public bool Available { get; set; }

        /// <summary>
        /// Track duration in ms
        /// </summary>
        [JsonProperty("durationMs")]
        public long DurationMs { get; set; }

        /// <summary>
        /// Track authors list
        /// </summary>
        [JsonProperty("artists")]
        public MetaArtist[] Artists { get; set; } = null!;

        /// <summary>
        /// List of albums that contain this track
        /// </summary>
        [JsonProperty("albums")]
        public MetaAlbumSignature[] Albums { get; set; } = null!;

        /// <summary>
        /// Cover link
        /// </summary>
        [JsonProperty("coverUri")]
        public string? CoverUri { get; set; }

        /// <summary>
        /// Opengraph image (alternative cover) link
        /// </summary>
        [JsonProperty("ogImage")]
        public string? OgImage { get; set; }

        /// <summary>
        /// Is lyrics available for this track
        /// </summary>
        [JsonProperty("lyricsAvailable")]
        public bool LyricsAvailable { get; set; }

        /// <summary>
        /// Convert this meta class to <see cref="AudioTrackInfo"/>
        /// </summary>
        /// <returns>Instance of <see cref="AudioTrackInfo"/></returns>
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
                    TimeSpan.FromMilliseconds(DurationMs),
                    Id.ToString(),
                    string.Format(TrackUrlFormat, album.Id, Id),
                    artworkUrl));
        }

        /// <inheritdoc />
        public Task<AudioTrackInfo> ToAudioTrackInfo(YandexMusicTrackLoader loader) {
            return ToAudioTrackInfo();
        }

        private static void TryApplyArtwork(ref string? final, string? artwork) {
            if (final != null || artwork == null) return;
            final = "https://" + artwork.Replace("%%", "200x200");
        }
    }
}