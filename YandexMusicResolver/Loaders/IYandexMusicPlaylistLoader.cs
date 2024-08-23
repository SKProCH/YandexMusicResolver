using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;

namespace YandexMusicResolver.Loaders;

/// <summary>
/// Represents class to getting playlists and albums from Yandex Music
/// </summary>
public interface IYandexMusicPlaylistLoader {
    /// <summary>
    /// Loads the playlist from Yandex Music
    /// </summary>
    /// <param name="userId">Id of user who created the playlist</param>
    /// <param name="playlistKind">Target playlist id</param>
    /// <returns>Playlist instance</returns>
    Task<YandexMusicPlaylist?> LoadPlaylist(string userId, string playlistKind);

    /// <summary>
    /// Loads the album from Yandex Music
    /// </summary>
    /// <param name="albumId">Target album id</param>
    /// <returns>Playlist instance</returns>
    Task<YandexMusicAlbum?> LoadAlbum(string albumId);
}