using System;
using System.Threading.Tasks;

namespace YandexMusicResolver.Config;

/// <summary>
/// Represents credential provider for Yandex
/// </summary>
public interface IYandexCredentialsProvider {
    /// <summary>
    /// Return token. Token can be cached.
    /// </summary>
    /// <returns>Yandex token. Actually token can be expired</returns>
    public Task<string?> GetTokenAsync();
    
    /// <summary>
    /// Validates token and retrieve a new one, if needed.
    /// </summary>
    /// <returns>Yandex token</returns>
    public Task<string?> ValidateOrRetrieveTokenAsync();

    /// <summary>
    /// Triggers when token changed
    /// </summary>
    public event EventHandler<YandexTokenChangedEventArgs> TokenChanged;
}
