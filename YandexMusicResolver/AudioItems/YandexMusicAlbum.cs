using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver.AudioItems {
    /// <summary>
    /// Represents album from Yandex Music
    /// </summary>
    public class YandexMusicAlbum : YandexMusicDataContainer<List<YandexMusicTrack>> {
        internal YandexMusicAlbum(long id, long year, List<YandexMusicArtist> artists, string? artworkUrl, long trackCount, string genre, string title,
                                  IYandexMusicPlaylistLoader loader) : base(async () => (await loader.LoadAlbum(id.ToString()))!.Data.ToList()) {
            Id = id;
            Year = year;
            Artists = artists;
            ArtworkUrl = artworkUrl;
            TrackCount = trackCount;
            Genre = genre;
            Title = title;
        }

        internal YandexMusicAlbum(long id, long year, List<YandexMusicArtist> artists, string? artworkUrl, long trackCount, string genre, string title,
                                  List<YandexMusicTrack> tracks) : base(tracks) {
            Id = id;
            Year = year;
            Artists = artists;
            ArtworkUrl = artworkUrl;
            TrackCount = trackCount;
            Genre = genre;
            Title = title;
        }


        /// <summary>
        /// Album id
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// Album release year
        /// </summary>
        public long Year { get; }

        /// <summary>
        /// Album artists
        /// </summary>
        public List<YandexMusicArtist> Artists { get; }

        /// <summary>
        /// Track image uri
        /// </summary>
        public string? ArtworkUrl { get; }

        /// <summary>
        /// Album tracks count
        /// </summary>
        public long TrackCount { get; }

        /// <summary>
        /// Album genre
        /// </summary>
        public string Genre { get; }

        /// <summary>
        /// Track title
        /// </summary>
        public string Title { get; }
    }
}