using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace YandexMusicResolver.Responces {
    internal class YandexApiResponse<T> {
        [JsonProperty("result")]
        public T? Result { get; set; }
        
        [JsonProperty("error")]
        public MetaError? Error { get; set; }
    }
}