using Newtonsoft.Json;

namespace YandexMusicResolver.Responces {
    public class MetaError
    {
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [JsonProperty("message")]
        public string Message { get; set; } = null!;
    }
}