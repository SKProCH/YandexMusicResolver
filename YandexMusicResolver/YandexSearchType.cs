namespace YandexMusicResolver;

/// <summary>
/// The type of entities that will be searched for
/// </summary>
public enum YandexSearchType {
    /// <summary>
    /// Only tracks
    /// </summary>
    Track,

    /// <summary>
    /// Only albums
    /// </summary>
    Album,

    /// <summary>
    /// Only playlists
    /// </summary>
    Playlist,

    /// <summary>
    /// All types
    /// </summary>
    All
}