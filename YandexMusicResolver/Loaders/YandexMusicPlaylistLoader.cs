using System;
using System.Linq;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Config;
using YandexMusicResolver.Requests;
using YandexMusicResolver.Responses;

namespace YandexMusicResolver.Loaders {
    /// <inheritdoc />
    public class YandexMusicPlaylistLoader : IYandexMusicPlaylistLoader {
        private const string PlaylistInfoFormat = "https://api.music.yandex.net/users/{0}/playlists/{1}";
        private const string AlbumInfoFormat = "https://api.music.yandex.net/albums/{0}/with-tracks";
        private readonly IYandexConfig _config;
        private readonly IYandexMusicTrackLoader _trackLoader;

        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicPlaylistLoader"/> class.
        /// </summary>
        /// <param name="config">Config instance for performing requests</param>
        [Obsolete("Some playlist cant be resolved without YandexMusicTrackLoader.\n" +
                  "Use another ctor.\n" +
                  "This left for backward compability and will be removed in next major version")]
        public YandexMusicPlaylistLoader(IYandexConfig config) {
            _config = config;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicPlaylistLoader"/> class.
        /// </summary>
        /// <param name="config">Config instance for performing requests</param>
        /// <param name="trackLoader">Instance of <see cref="YandexMusicTrackLoader"/> for resolving some strange playlists</param>
        public YandexMusicPlaylistLoader(IYandexConfig config, IYandexMusicTrackLoader trackLoader) {
            _trackLoader = trackLoader;
            _config = config;
        }

        /// <inheritdoc />
        public async Task<YandexMusicPlaylist?> LoadPlaylist(string userId, string playlistKind) {
            try {
                string url = string.Format(PlaylistInfoFormat, userId, playlistKind);
                var playlistData = await new YandexCustomRequest(_config).Create(url).GetResponseAsync<MetaPlaylist>();
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
                var playlistData = await new YandexCustomRequest(_config).Create(url).GetResponseAsync<MetaAlbum>();
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
