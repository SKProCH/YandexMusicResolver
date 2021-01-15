using System;
using System.Linq;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Config;
using YandexMusicResolver.Requests;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver.Loaders {
    /// <summary>
    /// Represents class to getting playlists and albums from Yandex Music
    /// </summary>
    public class YandexMusicPlaylistLoader : YandexMusicTrackLoader {
        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicPlaylistLoader"/> class.
        /// </summary>
        /// <param name="config">Config instance for performing requests</param>
        public YandexMusicPlaylistLoader(IYandexConfig config) : base(config) { }

        private const string PlaylistInfoFormat = "https://api.music.yandex.net/users/{0}/playlists/{1}";
        private const string AlbumInfoFormat = "https://api.music.yandex.net/albums/{0}/with-tracks";

        /// <summary>
        /// Loads the playlist from Yandex Music
        /// </summary>
        /// <param name="userId">Id of user who created the playlist</param>
        /// <param name="playlistId">Target playlist id</param>
        /// <param name="trackFactory">Track factory to create YandexMusicTrack from AudioTrackInfo</param>
        /// <returns>Playlist instance</returns>
        public Task<YandexMusicPlaylist?> LoadPlaylist(string userId, string playlistId, Func<AudioTrackInfo, YandexMusicTrack> trackFactory) {
            return LoadPlaylistUrl(string.Format(PlaylistInfoFormat, userId, playlistId), trackFactory);
        }

        /// <summary>
        /// Loads the album from Yandex Music
        /// </summary>
        /// <param name="albumId">Target album id</param>
        /// <param name="trackFactory">Track factory to create YandexMusicTrack from AudioTrackInfo</param>
        /// <returns>Playlist instance</returns>
        public Task<YandexMusicPlaylist?> LoadPlaylist(string albumId, Func<AudioTrackInfo, YandexMusicTrack> trackFactory) {
            return LoadPlaylistUrl(string.Format(AlbumInfoFormat, albumId), trackFactory);
        }

        private async Task<YandexMusicPlaylist?> LoadPlaylistUrl(string url, Func<AudioTrackInfo, YandexMusicTrack> trackFactory) {
            var playlistData = await new YandexCustomRequest(Config).Create(url).GetResponseAsync<MetaPlaylist>();
            if (playlistData.Tracks == null) {
                throw new Exception("Empty album or playlist found.");
            }

            var tracks = await Task.WhenAll(playlistData.Tracks.Select(async container => trackFactory(await container.ToAudioTrackInfo(this))));
            return new YandexMusicPlaylist(playlistData.Title, tracks.ToList().AsReadOnly(), false);
        }
    }
}