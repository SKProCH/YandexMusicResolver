using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver.Loaders {
    public class YandexMusicDirectUrlLoader {
        private const string TrackDownloadInfoFormat = "https://api.music.yandex.net/tracks/{0}/download-info";
        private const string DirectUrlFormat = "https://{0}/get-{1}/{2}/{3}{4}";
        private const string Mp3Salt = "XGRlBW9FXlekgbPrRHuSiA";

        public async Task<string> GetDirectUrl(string trackId, string codec) {
            var trackDownloadInfos = await WebRequestUtils.ExecuteGet(string.Format(TrackDownloadInfoFormat, trackId)).Parse<List<MetaTrackDownloadInfo>>();
            var track = trackDownloadInfos.FirstOrDefault(downloadInfo => downloadInfo.Codec == codec);
            if (track == null) {
                throw new Exception("Couldn't find supported track format.");
            }

            var downloadInfoContent = await WebRequestUtils.ExecuteGet("track.DownloadInfoUrl").GetContent();
            XmlSerializer serializer = new XmlSerializer(typeof(MetaTrackDownloadInfoXml));
            using StringReader reader = new StringReader(downloadInfoContent);
            var info = (MetaTrackDownloadInfoXml) serializer.Deserialize(reader);

            var sign = Utilities.CreateMD5(Mp3Salt + info.Path.Substring(1) + info.S);

            return string.Format(DirectUrlFormat, info.Host, codec, sign, info.Ts, info.Path);
        }
    }
}