using System.Xml.Serialization;

namespace YandexMusicResolver.Responces {
    [XmlRoot(ElementName = "download-info")]
    public class MetaTrackDownloadInfoXml {
        [XmlElement(ElementName = "host")] public string Host { get; set; }
        [XmlElement(ElementName = "path")] public string Path { get; set; }
        [XmlElement(ElementName = "ts")] public string Ts { get; set; }
        [XmlElement(ElementName = "region")] public string Region { get; set; }
        [XmlElement(ElementName = "s")] public string S { get; set; }
    }
}