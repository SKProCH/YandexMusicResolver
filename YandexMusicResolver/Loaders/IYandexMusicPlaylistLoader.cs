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
    /// <remarks>
    /// For use when you have playlist link like <c>https://music.yandex.ru/users/username/playlists/3</c>
    /// </remarks>
    Task<YandexMusicPlaylist?> LoadPlaylist(string userId, string playlistKind);

    /// <summary>
    /// Loads the playlist from Yandex Music
    /// </summary>
    /// <param name="playlistUuid">Id of playlist</param>
    /// <returns>Playlist instance</returns>
    /// <remarks>
    /// For use when you have playlist link like <c>https://music.yandex.ru/playlists/lk.e82a550e-63f9-4c8d-8ed0-ae15056051d8</c>
    /// </remarks>
    Task<YandexMusicPlaylist?> LoadPlaylist(string playlistUuid);

    /// <summary>
    /// Loads the album from Yandex Music
    /// </summary>
    /// <param name="albumId">Target album id</param>
    /// <returns>Playlist instance</returns>
    Task<YandexMusicAlbum?> LoadAlbum(string albumId);
}
