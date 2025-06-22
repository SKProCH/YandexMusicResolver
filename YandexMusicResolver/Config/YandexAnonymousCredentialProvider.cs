using System;
using System.Threading.Tasks;

namespace YandexMusicResolver.Config;

/// <inheritdoc />
public class YandexAnonymousCredentialProvider : IYandexCredentialsProvider {
    /// <summary>
    /// Default ctor
    /// </summary>
    protected YandexAnonymousCredentialProvider() { }
    
    /// <summary>
    /// Singleton instance
    /// </summary>
    public static YandexAnonymousCredentialProvider Instance { get; } = new();

    /// <inheritdoc />
    public Task<string?> GetTokenAsync() {
        return Task.FromResult<string?>(null);
    }

    /// <inheritdoc />
    public Task<string?> ValidateOrRetrieveTokenAsync() {
        return Task.FromResult<string?>(null);
    }

    /// <inheritdoc />
    public event EventHandler<YandexTokenChangedEventArgs>? TokenChanged;
}
