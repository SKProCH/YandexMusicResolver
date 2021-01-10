using Newtonsoft.Json;
#pragma warning disable 8618

namespace YandexMusicResolver.Responces {
    internal class MetaAccountResponse {
        [JsonProperty("account")]
        public MetaAccount Account { get; set; }
        
        [JsonProperty("defaultEmail")]
        public string DefaultEmail { get; set; }
    }
}