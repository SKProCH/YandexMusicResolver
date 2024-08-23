using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;
using YandexMusicResolver.Loaders;

namespace YandexMusicResolver {
    /// <summary>
    /// Represent main class for interacting with Yandex Music
    /// </summary>
    public interface IYandexMusicMainResolver {
        /// <summary>
        /// Instance of <see cref="YandexMusicPlaylistLoader"/>
        /// </summary>
        IYandexMusicPlaylistLoader PlaylistLoader { get; }

        /// <summary>
        /// Instance of <see cref="YandexMusicTrackLoader"/>
        /// </summary>
        IYandexMusicTrackLoader TrackLoader { get; }

        /// <summary>
        /// Instance of <see cref="YandexMusicDirectUrlLoader"/>
        /// </summary>
        IYandexMusicDirectUrlLoader DirectUrlLoader { get; }

        /// <summary>
        /// Instance of <see cref="YandexMusicSearchResultLoader"/>
        /// </summary>
        IYandexMusicSearchResultLoader SearchResultLoader { get; }

        /// <summary>
        /// Is complicated query in <see cref="ResolveQuery"/> can be resolved
        /// </summary>
        bool AllowSearch { get; set; }

        /// <summary>
        /// If we pass plain text to <see cref="ResolveQuery"/> it will be interpreted as search query with this search type. Set <see cref="PlainTextIsSearchQuery"/> to <code>false</code> to disable this
        /// </summary>
        YandexSearchType PlainTextIsSearchQueryType { get; set; }

        /// <summary>
        /// Will plain text be interpreted as a search query in <see cref="ResolveQuery"/>
        /// </summary>
        bool PlainTextIsSearchQuery { get; set; }

        /// <summary>
        /// Resolves yandex query. Can directly resolve playlists, albums, tracks by url and search queries
        /// </summary>
        /// <param name="query">Direct url or search query</param>
        /// <param name="allowSearchOverride">Is query in <see cref="YandexMusicMainResolver.ResolveQuery"/> can be resolved with search. This parameter overrides <see cref="YandexMusicMainResolver.AllowSearch"/></param>
        /// <param name="plainTextIsSearchQueryOverride">Will plain text be interpreted as a search query in <see cref="YandexMusicMainResolver.ResolveQuery"/></param>
        /// <param name="plainTextAsSearchQueryTypeOverride">If we pass plain text to <see cref="YandexMusicMainResolver.ResolveQuery"/> it will be interpreted as search query with this search type</param>
        /// <returns>Instance of <see cref="YandexMusicSearchResult"/>. Null if track will now an valid url and <see cref="YandexMusicMainResolver.AllowSearch"/> is false.</returns>
        Task<YandexMusicSearchResult?> ResolveQuery(string query, bool? allowSearchOverride = null,
                                                    bool? plainTextIsSearchQueryOverride = null,
                                                    YandexSearchType? plainTextAsSearchQueryTypeOverride = null);

        /// <summary>
        /// Determine is query can be resolved with target parameters
        /// </summary>
        /// <param name="query">Direct url or search query</param>
        /// <param name="allowSearchOverride">Is query in <see cref="YandexMusicMainResolver.ResolveQuery"/> can be resolved with search. This parameter overrides <see cref="YandexMusicMainResolver.AllowSearch"/></param>
        /// <param name="plainTextIsSearchQueryOverride">Will plain text be interpreted as a search query in <see cref="YandexMusicMainResolver.ResolveQuery"/></param>
        /// <param name="plainTextAsSearchQueryTypeOverride">If we pass plain text to <see cref="YandexMusicMainResolver.ResolveQuery"/> it will be interpreted as search query with this search type</param>
        /// <returns>True if query can be resolved</returns>
        bool CanResolveQuery(string query, bool? allowSearchOverride = null,
                             bool? plainTextIsSearchQueryOverride = null,
                             YandexSearchType? plainTextAsSearchQueryTypeOverride = null);
    }
}
