using Newtonsoft.Json;

namespace YandexMusicResolver.Responces {
    public class MetaError
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}