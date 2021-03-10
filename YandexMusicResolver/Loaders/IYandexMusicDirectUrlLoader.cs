using System;
using System.Threading.Tasks;

namespace YandexMusicResolver.Loaders {
    public interface IYandexMusicDirectUrlLoader {
        /// <summary>
        /// Get direct url to download track
        /// </summary>
        /// <remarks>If you not authorized will return 30s track version. This is YandexMusic restriction</remarks>
        /// <param name="trackId">Target track id</param>
        /// <param name="codec">Target codec. mp3 by default</param>
        /// <returns>Direct url to download track</returns>
        /// <exception cref="Exception">Couldn't find supported track format</exception>
        Task<string> GetDirectUrl(string trackId, string codec = "mp3");
    }
}