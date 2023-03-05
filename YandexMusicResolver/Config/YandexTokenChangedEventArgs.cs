using System;

namespace YandexMusicResolver.Config;

/// <summary>
/// Contains token for <see cref="IYandexCredentialsProvider.TokenChanged"/>
/// </summary>
public sealed class YandexTokenChangedEventArgs : EventArgs {
    /// <inheritdoc />
    internal YandexTokenChangedEventArgs(string token) {
        Token = token;
    }
    public string Token { get; }
}
