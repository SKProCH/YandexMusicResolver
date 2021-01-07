using System.Collections.Generic;
using Newtonsoft.Json;

namespace YandexMusicResolver.Responces {
    public class MetaArtist
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}