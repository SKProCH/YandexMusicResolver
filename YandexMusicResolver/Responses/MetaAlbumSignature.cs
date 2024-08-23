using System.Collections.Generic;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.Responses;

internal class MetaAlbumSignature {
    public long Id { get; set; }

    public long Year { get; set; }

    public List<YandexMusicArtist> Artists { get; set; } = null!;

    public string? CoverUri { get; set; }

    public long TrackCount { get; set; }

    public string Genre { get; set; } = null!;

    public bool Available { get; set; }

    public bool AvailableForPremiumUsers { get; set; }

    public string Title { get; set; }

    public virtual YandexMusicAlbum ToYmAlbum(IYandexMusicPlaylistLoader loader) {
        return new(Id, Year, Artists, CoverUri, TrackCount, Genre, Title, loader);
    }
}