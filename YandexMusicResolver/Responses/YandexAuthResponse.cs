using System;

namespace YandexMusicResolver.Responses;

/// <summary>
/// Response from auth
/// </summary>
public class YandexAuthResponse {
    /// <summary>
    /// Access token
    /// </summary>
    public string AccessToken { get; } = null!;

    /// <summary>
    /// Expires in (Unix time seconds)
    /// </summary>
    public long ExpiresIn { get; }
        
    /// <summary>
    /// <see cref="ExpiresIn"/> parsed
    /// </summary>
    public DateTimeOffset ExpiresInDateTime => DateTimeOffset.FromUnixTimeSeconds(ExpiresIn);

    /// <summary>
    /// Token type
    /// </summary>
    public string TokenType { get; } = null!;

    /// <summary>
    /// User id
    /// </summary>
    public long Uid { get; }
}