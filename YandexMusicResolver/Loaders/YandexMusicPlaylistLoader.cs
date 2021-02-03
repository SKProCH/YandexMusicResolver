using System;
using System.Linq;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Config;
using YandexMusicResolver.Requests;
using YandexMusicResolver.Responses;

namespace YandexMusicResolver.Loaders {
    /// <summary>
    /// Represents class to getting playlists and albums from Yandex Music
    /// </summary>
    public class YandexMusicPlaylistLoader {
        protected readonly IYandexConfig Config;

        /// <summary>
        /// Initializes a new instance of the <see cref="YandexMusicPlaylistLoader"/> class.
        /// </summary>
        /// <param name="config">Config instance for performing requests</param>
        public YandexMusicPlaylistLoader(IYandexConfig config) {
            Config = config;
        }

        private const string PlaylistInfoFormat = "https://api.music.yandex.net/users/{0}/playlists/{1}";
        private const string AlbumInfoFormat = "https://api.music.yandex.net/albums/{0}/with-tracks";

        /// <summary>
        /// Loads the playlist from Yandex Music
        /// </summary>
        /// <param name="userId">Id of user who created the playlist</param>
        /// <param name="playlistKind">Target playlist id</param>
        /// <returns>Playlist instance</returns>
        public async Task<YandexMusicPlaylist?> LoadPlaylist(string userId, string playlistKind) {
            string url = string.Format(PlaylistInfoFormat, userId, playlistKind);
            var playlistData = await new YandexCustomRequest(Config).Create(url).GetResponseAsync<MetaPlaylist>();
            if (playlistData.Tracks == null) {
                throw new Exception("Empty playlist found.");
            }

            return playlistData.ToYaPlaylist(this);
        }
        
        /// <summary>
        /// Loads the album from Yandex Music
        /// </summary>
        /// <param name="albumId">Target album id</param>
        /// <returns>Playlist instance</returns>
        public async Task<YandexMusicAlbum?> LoadAlbum(string albumId) {
            string url = string.Format(AlbumInfoFormat, albumId);
            var playlistData = await new YandexCustomRequest(Config).Create(url).GetResponseAsync<MetaAlbum>();
            if (playlistData.Tracks == null) {
                throw new Exception("Empty album or playlist found.");
            }

            return playlistData.ToYmAlbum(this);
        }
    }
}