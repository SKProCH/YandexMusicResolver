using Newtonsoft.Json;

namespace YandexMusicResolver.Responses {
    internal class MetaCover {
        [JsonProperty("type")]
        public string Type { get; set; } = null!;

        [JsonProperty("dir")]
        public string? Dir { get; set; }

        [JsonProperty("version")]
        public string? Version { get; set; }

        [JsonProperty("uri")]
        public string? Uri { get; set; }

        [JsonProperty("custom")]
        public bool Custom { get; set; }

        public string? GetCoverUrl() {
            if (Uri == null) return null;
            return "https://" + Uri.Replace("%%", "200x200");
        }
    }
}