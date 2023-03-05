using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using YandexMusicResolver.Config;
using YandexMusicResolver.Responses;

namespace YandexMusicResolver.Loaders {
    /// <inheritdoc />
    public class YandexMusicDirectUrlLoader : IYandexMusicDirectUrlLoader {
        private const string TrackDownloadInfoFormat = "https://api.music.yandex.net/tracks/{0}/download-info";
        private const string DirectUrlFormat = "https://{0}/get-{1}/{2}/{3}{4}";
        private const string Mp3Salt = "XGRlBW9FXlekgbPrRHuSiA";
        private IYandexCredentialsProvider _credentialsProvider;
        private HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicDirectUrlLoader"/> class.
        /// </summary>
        /// <param name="credentialsProvider">Config instance for performing requests</param>
        /// <param name="httpClientFactory">Factory for resolving HttpClient. Client name is <see cref="YandexMusicUtilities.HttpClientName"/></param>
        public YandexMusicDirectUrlLoader(IYandexCredentialsProvider credentialsProvider, IHttpClientFactory httpClientFactory) {
            _httpClient = httpClientFactory.GetYMusicHttpClient();
            _credentialsProvider = credentialsProvider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicDirectUrlLoader"/> class.
        /// </summary>
        /// <param name="credentialsProvider">Config instance for performing requests</param>
        /// <param name="httpClient">HttpClient for performing requests. But preferred way is use another ctor and pass <see cref="IHttpClientFactory"/></param>
        public YandexMusicDirectUrlLoader(IYandexCredentialsProvider credentialsProvider, HttpClient httpClient) {
            _httpClient = httpClient;
            _credentialsProvider = credentialsProvider;
        }

        /// <inheritdoc />
        public async Task<string> GetDirectUrl(string trackId, string codec = "mp3") {
            try {
                var url = string.Format(TrackDownloadInfoFormat, trackId);
                var trackDownloadInfos = await _httpClient.PerformYMusicRequestAsync<List<MetaTrackDownloadInfo>>(_credentialsProvider, url);
                var track = trackDownloadInfos.FirstOrDefault(downloadInfo => downloadInfo.Codec == codec);
                if (track == null) {
                    throw new Exception("Couldn't find supported track format.");
                }

                var downloadInfoContent = await _httpClient.PerformYMusicRequestAsync(_credentialsProvider, track.DownloadInfoUrl.ToString());
                var serializer = new XmlSerializer(typeof(MetaTrackDownloadInfoXml));
                var info = (MetaTrackDownloadInfoXml)serializer.Deserialize(await downloadInfoContent.Content.ReadAsStreamAsync());

                var sign = YandexMusicUtilities.CreateMd5(Mp3Salt + info.Path.Substring(1) + info.S);

                return string.Format(DirectUrlFormat, info.Host, codec, sign, info.Ts, info.Path);
            }
            catch (Exception e) {
                throw new YandexMusicException("Exception while resolving direct url", e);
            }
        }
    }
}
