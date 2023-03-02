using System;

namespace YandexMusicResolver.Responses {
    internal class MetaTrackDownloadInfo {
        public string Codec { get; set; } = null!;

        public bool Gain { get; set; }

        public bool Preview { get; set; }

        public Uri DownloadInfoUrl { get; set; } = null!;

        public bool Direct { get; set; }

        public long BitrateInKbps { get; set; }
    }
}
