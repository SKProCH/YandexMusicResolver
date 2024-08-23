using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Ids;

namespace YandexMusicResolver.Responses;

/// <summary>
/// Track data from Yandex Music
/// </summary>
internal class MetaTrack {
    private const string TrackUrlFormat = "https://music.yandex.ru/album/{0}/track/{1}";

    [JsonConverter(typeof(YandexIdConverter))]
    public YandexId Id { get; set; }

    public string Title { get; set; } = null!;

    public bool Available { get; set; }

    public long DurationMs { get; set; }

    public List<YandexMusicArtist> Artists { get; set; } = null!;

    public List<MetaAlbumSignature> Albums { get; set; } = null!;

    public string? CoverUri { get; set; }

    public string? OgImage { get; set; }

    public bool LyricsAvailable { get; set; }

    public YandexMusicTrack ToYmTrack() {
        var album = Albums.FirstOrDefault();

        string? artworkUrl = null;
        TryApplyArtwork(ref artworkUrl, CoverUri);
        TryApplyArtwork(ref artworkUrl, OgImage);
        TryApplyArtwork(ref artworkUrl, album?.CoverUri);

        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        var uri = album != null ? string.Format(TrackUrlFormat, album.Id, Id) : null;
        return new YandexMusicTrack(Title, Artists,
            TimeSpan.FromMilliseconds(DurationMs), Id,
            uri, Available, artworkUrl);
    }

    private static void TryApplyArtwork(ref string? final, string? artwork) {
        if (final != null || artwork == null) return;
        final = "https://" + artwork.Replace("%%", "200x200");
    }
}