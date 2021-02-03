using System.Collections.Generic;
using Newtonsoft.Json;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responses {
    internal class MetaAlbumSignature
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("year")]
        public long Year { get; set; }

        [JsonProperty("artists")]
        public List<YandexMusicArtist> Artists { get; set; } = null!;

        [JsonProperty("coverUri")]
        public string? CoverUri { get; set; }

        [JsonProperty("trackCount")]
        public long TrackCount { get; set; }

        [JsonProperty("genre")]
        public string Genre { get; set; } = null!;

        [JsonProperty("available")]
        public bool Available { get; set; }

        [JsonProperty("availableForPremiumUsers")]
        public bool AvailableForPremiumUsers { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        public virtual YandexMusicAlbum ToYmAlbum(YandexMusicPlaylistLoader loader) {
            return new(Id, Year, Artists, CoverUri, TrackCount, Genre, Title, loader);
        }
    }
}