using System.Xml.Serialization;

#pragma warning disable 1591

namespace YandexMusicResolver.Responses {
    [XmlRoot(ElementName = "download-info")]
    public class MetaTrackDownloadInfoXml {
        [XmlElement(ElementName = "host")]
        public string Host { get; set; } = null!;

        [XmlElement(ElementName = "path")]
        public string Path { get; set; } = null!;

        [XmlElement(ElementName = "ts")]
        public string Ts { get; set; } = null!;

        [XmlElement(ElementName = "region")]
        public string Region { get; set; } = null!;

        [XmlElement(ElementName = "s")]
        public string S { get; set; } = null!;
    }
}
