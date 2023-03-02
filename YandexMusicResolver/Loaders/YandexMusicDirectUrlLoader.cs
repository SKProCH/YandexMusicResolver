using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using YandexMusicResolver.Config;
using YandexMusicResolver.Requests;
using YandexMusicResolver.Responses;

namespace YandexMusicResolver.Loaders {
    /// <inheritdoc />
    public class YandexMusicDirectUrlLoader : IYandexMusicDirectUrlLoader {
        private const string TrackDownloadInfoFormat = "https://api.music.yandex.net/tracks/{0}/download-info";
        private const string DirectUrlFormat = "https://{0}/get-{1}/{2}/{3}{4}";
        private const string Mp3Salt = "XGRlBW9FXlekgbPrRHuSiA";
        private IYandexConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicDirectUrlLoader"/> class.
        /// </summary>
        /// <param name="config">Config instance for performing requests</param>
        public YandexMusicDirectUrlLoader(IYandexConfig config) {
            config.Load();
            _config = config;
        }

        /// <inheritdoc />
        public async Task<string> GetDirectUrl(string trackId, string codec = "mp3") {
            try {
                var trackDownloadInfos = await new YandexCustomRequest(_config).Create(string.Format(TrackDownloadInfoFormat, trackId))
                    .GetResponseAsync<List<MetaTrackDownloadInfo>>();
                var track = trackDownloadInfos.FirstOrDefault(downloadInfo => downloadInfo.Codec == codec);
                if (track == null) {
                    throw new Exception("Couldn't find supported track format.");
                }

                var downloadInfoContent = await new YandexCustomRequest(_config).Create(track.DownloadInfoUrl.ToString()).GetResponseBodyAsync();
                var serializer = new XmlSerializer(typeof(MetaTrackDownloadInfoXml));
                using var reader = new StringReader(downloadInfoContent);
                var info = (MetaTrackDownloadInfoXml)serializer.Deserialize(reader);

                var sign = Utilities.CreateMd5(Mp3Salt + info.Path[1..] + info.S);

                return string.Format(DirectUrlFormat, info.Host, codec, sign, info.Ts, info.Path);
            }
            catch (Exception e) {
                throw new YandexMusicException("Exception while resolving direct url", e);
            }
        }
    }
}
