using Newtonsoft.Json;

namespace YandexMusicResolver.Responces {
    /// <summary>
    /// Represent a artist in Yandex Music
    /// </summary>
    public class MetaArtist {
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