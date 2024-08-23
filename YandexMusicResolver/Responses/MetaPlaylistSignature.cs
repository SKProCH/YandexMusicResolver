using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responses;

internal class MetaPlaylistSignature {
    public long Uid { get; set; }

    public long Kind { get; set; }

    public long TrackCount { get; set; }

    public string Title { get; set; }

    public YandexMusicOwner Owner { get; set; }

    public MetaCover Cover { get; set; }

    public virtual YandexMusicPlaylist ToYaPlaylist(IYandexMusicPlaylistLoader loader) {
        return new(Uid, Kind, TrackCount, Title, Owner, Cover.GetCoverUrl(), loader);
    }
}