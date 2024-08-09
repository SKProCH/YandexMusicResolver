using System.Text.Json.Serialization;
using YandexMusicResolver.Ids;

namespace YandexMusicResolver.AudioItems {
    /// <summary>
    /// Represent a artist in Yandex Music
    /// </summary>
    public class YandexMusicArtist {
        /// <summary>
        /// Artist ID
        /// </summary>
        [JsonConverter(typeof(YandexIdConverter))]
        public YandexId Id { get; set; }

        /// <summary>
        /// Artist name
        /// </summary>
        public string Name { get; set; } = null!;
    }
}
