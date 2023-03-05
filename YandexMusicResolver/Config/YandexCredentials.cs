namespace YandexMusicResolver.Config;

/// <summary>
/// Class for storing credentials for <see cref="YandexCredentialsProvider"/>
/// </summary>
public class YandexCredentials {
    /// <summary>
    /// Login for authorizing in Yandex
    /// </summary>
    public string? Login { get; set; }

    /// <summary>
    /// Password for authorizing in Yandex
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// Token for authorizing in Yandex
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Is anonymous access allowed
    /// </summary>
    public bool AllowAnonymous { get; set; }
}
