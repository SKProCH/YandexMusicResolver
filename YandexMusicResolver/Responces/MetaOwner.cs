using Newtonsoft.Json;

namespace YandexMusicResolver.Responces {
    public class MetaOwner {
        [JsonProperty("uid")]
        public long Uid { get; set; }

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }
    }
}