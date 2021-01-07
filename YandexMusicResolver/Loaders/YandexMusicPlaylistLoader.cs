using System;
using System.Linq;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver.Loaders {
    public class YandexMusicPlaylistLoader : YandexMusicTrackLoader {
        private const string PlaylistInfoFormat = "https://api.music.yandex.net/users/{0}/playlists/{1}";
        private const string AlbumInfoFormat = "https://api.music.yandex.net/albums/{0}/with-tracks";

        public Task<YandexMusicPlaylist?> LoadPlaylist(string userId, string playlistId, Func<AudioTrackInfo, YandexMusicTrack> trackFactory) {
            return LoadPlaylistUrl(string.Format(PlaylistInfoFormat, userId, playlistId), trackFactory);
        }

        public Task<YandexMusicPlaylist?> LoadPlaylist(string albumId, Func<AudioTrackInfo, YandexMusicTrack> trackFactory) {
            return LoadPlaylistUrl(string.Format(AlbumInfoFormat, albumId), trackFactory);
        }

        private async Task<YandexMusicPlaylist?> LoadPlaylistUrl(string url, Func<AudioTrackInfo, YandexMusicTrack> trackFactory) {
            var playlistData = await WebRequestUtils.ExecuteGet(url).Parse<MetaPlaylist>();
            if (playlistData.Tracks == null) {
                throw new Exception("Empty album or playlist found.");
            }

            var tracks = await Task.WhenAll(playlistData.Tracks.Select(async container => trackFactory(await container.ToAudioTrackInfo(this))));
            return new YandexMusicPlaylist(playlistData.Title, tracks.ToList().AsReadOnly(), false);
        }
    }
}