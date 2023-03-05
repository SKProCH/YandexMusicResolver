using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Config;
using YandexMusicResolver.Responses;

namespace YandexMusicResolver.Loaders {
    /// <inheritdoc />
    public class YandexMusicPlaylistLoader : IYandexMusicPlaylistLoader {
        private const string PlaylistInfoFormat = "https://api.music.yandex.net/users/{0}/playlists/{1}";
        private const string AlbumInfoFormat = "https://api.music.yandex.net/albums/{0}/with-tracks";
        private readonly IYandexCredentialsProvider _yandexCredentialsProvider;
        private readonly IYandexMusicTrackLoader? _trackLoader;
        private HttpClient _httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicPlaylistLoader"/> class.
        /// </summary>
        /// <param name="yandexCredentialsProvider">Config instance for performing requests</param>
        /// <param name="httpClientFactory">Factory for resolving HttpClient. Client name is <see cref="YandexMusicUtilities.HttpClientName"/></param>
        /// <param name="trackLoader">Instance of <see cref="YandexMusicTrackLoader"/> for resolving some strange playlists</param>
        public YandexMusicPlaylistLoader(IYandexCredentialsProvider yandexCredentialsProvider, IHttpClientFactory httpClientFactory, IYandexMusicTrackLoader? trackLoader = null) {
            _httpClient = httpClientFactory.GetYMusicHttpClient();
            _trackLoader = trackLoader;
            _yandexCredentialsProvider = yandexCredentialsProvider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicPlaylistLoader"/> class.
        /// </summary>
        /// <param name="yandexCredentialsProvider">Config instance for performing requests</param>
        /// <param name="httpClient">HttpClient for performing requests. But preferred way is use another ctor and pass <see cref="IHttpClientFactory"/></param>
        /// <param name="trackLoader">Instance of <see cref="YandexMusicTrackLoader"/> for resolving some strange playlists</param>
        public YandexMusicPlaylistLoader(IYandexCredentialsProvider yandexCredentialsProvider, HttpClient httpClient, IYandexMusicTrackLoader? trackLoader = null) {
            _httpClient = httpClient;
            _trackLoader = trackLoader;
            _yandexCredentialsProvider = yandexCredentialsProvider;
        }

        /// <inheritdoc />
        public async Task<YandexMusicPlaylist?> LoadPlaylist(string userId, string playlistKind) {
            try {
                string url = string.Format(PlaylistInfoFormat, userId, playlistKind);
                var playlistData = await _httpClient.PerformYMusicRequestAsync<MetaPlaylist>(_yandexCredentialsProvider, url);
                if (playlistData.Tracks == null) {
                    throw new Exception("Empty playlist found.");
                }

                if (playlistData.Tracks.Any(container => container.Track == null)) {
                    if (_trackLoader == null) {
                        throw new Exception("Yandex Music for some reason gives a different response format (no tracks) for some playlists. " +
                                            "Use another ctor and pass YandexMusicTrackLoader to ctor to get proper response");
                    }

                    var trackIds = playlistData.Tracks
                        .Where(pair => pair.Track == null)
                        .Select(pair => pair.Id);
                    var yandexMusicTracks = await _trackLoader.LoadTracks(trackIds);

                    return playlistData.ToYaPlaylist(yandexMusicTracks);
                }

                return playlistData.ToYaPlaylist(this);
            }
            catch (Exception e) {
                throw new YandexMusicException("Exception while loading playlist", e);
            }
        }

        /// <inheritdoc />
        public async Task<YandexMusicAlbum?> LoadAlbum(string albumId) {
            try {
                string url = string.Format(AlbumInfoFormat, albumId);
                var playlistData = await _httpClient.PerformYMusicRequestAsync<MetaAlbum>(_yandexCredentialsProvider, url);
                if (playlistData.Tracks == null) {
                    throw new Exception("Empty album or playlist found.");
                }

                return playlistData.ToYmAlbum(this);
            }
            catch (Exception e) {
                throw new YandexMusicException("Exception while loading album", e);
            }
        }
    }
}
