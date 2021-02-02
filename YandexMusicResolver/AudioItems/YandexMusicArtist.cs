using Newtonsoft.Json;

namespace YandexMusicResolver.AudioItems {
    /// <summary>
    /// Represent a artist in Yandex Music
    /// </summary>
    public class YandexMusicArtist {
        /// <summary>
        /// Artist ID
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// Artist name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = null!;
    }
}