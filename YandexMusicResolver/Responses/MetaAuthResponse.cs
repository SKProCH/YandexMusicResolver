using Newtonsoft.Json;

namespace YandexMusicResolver.Responses {
    internal class MetaAuthResponse {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = null!;

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; } = null!;

        [JsonProperty("uid")]
        public long Uid { get; set; }
    }
}