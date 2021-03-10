using System;
using System.Threading.Tasks;
using YandexMusicResolver.AudioItems;

namespace YandexMusicResolver.Loaders {
    public interface IYandexMusicSearchResultLoader {
        /// <summary>
        /// Special prefix for complicated requests
        /// </summary>
        string SearchPrefix { get; }

        /// <summary>
        /// Set a new search prefix for complicated queries
        /// </summary>
        /// <param name="prefix">New prefix. <code>null</code> will be replaced with "ymsearch"</param>
        void SetSearchPrefix(string? prefix = null);

        /// <summary>
        /// Perform search request on Yandex Music
        /// </summary>
        /// <remarks>Complicated query is <see cref="YandexMusicSearchResultLoader.SearchPrefix"/>:<see cref="YandexSearchType"/>:limit:text</remarks>
        /// <param name="query">Search query. May be complicated or default values will be used</param>
        /// <returns>Instance of YandexMusicSearchResult</returns>
        Task<YandexMusicSearchResult?> LoadSearchResult(string query);

        /// <summary>
        /// Perform search request on Yandex Music
        /// </summary>
        /// <param name="type">Search type</param>
        /// <param name="query">Search text</param>
        /// <param name="limit">Search results limit count</param>
        /// <returns>Instance of YandexMusicSearchResult</returns>
        /// <exception cref="Exception">Throws exception if something went wrong</exception>
        Task<YandexMusicSearchResult?> LoadSearchResult(YandexSearchType type, string query, int limit = YandexMusicSearchResultLoader.DefaultLimit);

        /// <summary>
        /// Parse complicated query into pieces
        /// </summary>
        /// <remarks>Complicated query is <see cref="YandexMusicSearchResultLoader.SearchPrefix"/>:<see cref="YandexSearchType"/>:limit:text</remarks>
        /// <param name="query">Target query</param>
        /// <param name="text">Search text</param>
        /// <param name="type">Search type</param>
        /// <param name="limit">Search limit</param>
        /// <returns>True if is this complicated query</returns>
        bool TryParseQuery(string query, out string text, out YandexSearchType type, out int limit);
    }
}