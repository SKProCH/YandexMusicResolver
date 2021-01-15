using System;
using Newtonsoft.Json;

namespace YandexMusicResolver.Responces {
    internal class MetaTrackDownloadInfo {
        [JsonProperty("codec")]
        public string Codec { get; set; } = null!;

        [JsonProperty("gain")]
        public bool Gain { get; set; }

        [JsonProperty("preview")]
        public bool Preview { get; set; }

        [JsonProperty("downloadInfoUrl")]
        public Uri DownloadInfoUrl { get; set; } = null!;

        [JsonProperty("direct")]
        public bool Direct { get; set; }

        [JsonProperty("bitrateInKbps")]
        public long BitrateInKbps { get; set; }
    }
}