using System.Collections.Generic;
using System.Linq;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.AudioItems;

/// <summary>
/// Represents album from Yandex Music
/// </summary>
public class YandexMusicPlaylist : YandexMusicDataContainer<IReadOnlyCollection<YandexMusicTrack>> {
    internal YandexMusicPlaylist(long uid, long kind, long trackCount, string title, YandexMusicOwner owner, string? artworkUrl,
        IYandexMusicPlaylistLoader loader) :
        base(async () => (await loader.LoadPlaylist(owner.Login, kind.ToString()))!.Data.ToList()) {
        Uid = uid;
        Kind = kind;
        TrackCount = trackCount;
        Title = title;
        Owner = owner;
        ArtworkUrl = artworkUrl;
    }

    internal YandexMusicPlaylist(long uid, long kind, long trackCount, string title, YandexMusicOwner owner, string? artworkUrl,
        IReadOnlyCollection<YandexMusicTrack> tracks) : base(tracks) {
        Uid = uid;
        Kind = kind;
        TrackCount = trackCount;
        Title = title;
        Owner = owner;
        ArtworkUrl = artworkUrl;
    }

    /// <summary>
    /// Playlist UID
    /// </summary>
    public long Uid { get; }

    /// <summary>
    /// Playlist kind (something like the user's playlist index)
    /// </summary>
    public long Kind { get; }

    /// <summary>
    /// Playlist tracks count
    /// </summary>
    public long TrackCount { get; }

    /// <summary>
    /// Playlist title
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Playlist owner
    /// </summary>
    public YandexMusicOwner Owner { get; }

    /// <summary>
    /// Playlist artwork url
    /// </summary>
    public string? ArtworkUrl { get; }
}