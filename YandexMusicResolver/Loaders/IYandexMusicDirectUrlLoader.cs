using System;
using System.Threading.Tasks;

namespace YandexMusicResolver.Loaders;

/// <summary>
/// Represents class to getting direct links from tracks
/// </summary>
public interface IYandexMusicDirectUrlLoader {
    /// <summary>
    /// Get direct url to download track
    /// </summary>
    /// <remarks>If you not authorized will return 30s track version. This is YandexMusic restriction</remarks>
    /// <param name="trackId">Target track id</param>
    /// <param name="codec">Target codec. mp3 by default</param>
    /// <returns>Direct url to download track</returns>
    /// <exception cref="Exception">Couldn't find supported track format</exception>
    Task<string> GetDirectUrl(long trackId, string codec = "mp3");
}
