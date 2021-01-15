using Newtonsoft.Json;

namespace YandexMusicResolver.Responces {
    /// <summary>
    /// Represent playlist owner
    /// </summary>
    public class MetaOwner {
        /// <summary>
        /// Owner ID
        /// </summary>
        [JsonProperty("uid")]
        public long Uid { get; set; }

        /// <summary>
        /// Owner login
        /// </summary>
        [JsonProperty("login")]
        public string Login { get; set; } = null!;

        /// <summary>
        /// Owner name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// Is owner verified
        /// </summary>
        [JsonProperty("verified")]
        public bool Verified { get; set; }
    }
}