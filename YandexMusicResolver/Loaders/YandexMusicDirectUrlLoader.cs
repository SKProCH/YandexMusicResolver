using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using YandexMusicResolver.Config;
using YandexMusicResolver.Requests;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver.Loaders {
    /// <summary>
    /// Represents class to getting direct links from tracks
    /// </summary>
    public class YandexMusicDirectUrlLoader {
        private IYandexConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicDirectUrlLoader"/> class.
        /// </summary>
        /// <param name="config">Config instance for performing requests</param>
        public YandexMusicDirectUrlLoader(IYandexConfig config) {
            _config = config;
        }

        private const string TrackDownloadInfoFormat = "https://api.music.yandex.net/tracks/{0}/download-info";
        private const string DirectUrlFormat = "https://{0}/get-{1}/{2}/{3}{4}";
        private const string Mp3Salt = "XGRlBW9FXlekgbPrRHuSiA";

        /// <summary>
        /// Get direct url to download track
        /// </summary>
        /// <remarks>If you not authorized will return 30s track version. This is YandexMusic restriction</remarks>
        /// <param name="trackId">Target track id</param>
        /// <param name="codec">Target codec. mp3 by default</param>
        /// <returns>Direct url to download track</returns>
        /// <exception cref="Exception">Couldn't find supported track format</exception>
        public async Task<string> GetDirectUrl(string trackId, string codec) {
            var trackDownloadInfos = await new YandexCustomRequest(_config).Create(string.Format(TrackDownloadInfoFormat, trackId))
                                                                           .GetResponseAsync<List<MetaTrackDownloadInfo>>();
            var track = trackDownloadInfos.FirstOrDefault(downloadInfo => downloadInfo.Codec == codec);
            if (track == null) {
                throw new Exception("Couldn't find supported track format.");
            }

            var downloadInfoContent = await new YandexCustomRequest(_config).Create(track.DownloadInfoUrl.ToString()).GetResponseBodyAsync();
            var serializer = new XmlSerializer(typeof(MetaTrackDownloadInfoXml));
            using var reader = new StringReader(downloadInfoContent);
            var info = (MetaTrackDownloadInfoXml) serializer.Deserialize(reader);

            var sign = Utilities.CreateMd5(Mp3Salt + info.Path.Substring(1) + info.S);

            return string.Format(DirectUrlFormat, info.Host, codec, sign, info.Ts, info.Path);
        }
    }
}