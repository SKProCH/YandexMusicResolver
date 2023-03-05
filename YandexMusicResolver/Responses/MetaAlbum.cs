using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responses {
    internal class MetaAlbum : MetaAlbumSignature {
        [JsonPropertyName("volumes")]
        public List<List<MetaTrack>> Tracks { get; set; } = new();

        public string OgImage { get; set; }

        public override YandexMusicAlbum ToYmAlbum(IYandexMusicPlaylistLoader loader) {
            var artwork = CoverUri;
            if (string.IsNullOrEmpty(artwork)) {
                artwork = OgImage;
            }

            return new YandexMusicAlbum(Id, Year, Artists, artwork, TrackCount, Genre, Title,
                Tracks.SelectMany(list => list).Select(track => track.ToYmTrack()).ToList());
        }
    }
}
