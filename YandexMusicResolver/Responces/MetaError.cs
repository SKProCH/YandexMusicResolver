using Newtonsoft.Json;

namespace YandexMusicResolver.Responces {
    /// <summary>
    /// Represents error that returned from Yandex Music
    /// </summary>
    public class MetaError {
        /// <summary>
        /// Error name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Error message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; } = null!;
    }
}