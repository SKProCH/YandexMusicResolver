namespace YandexMusicResolver.AudioItems;

/// <summary>
/// Represent playlist owner
/// </summary>
public class YandexMusicOwner {
    /// <summary>
    /// Owner ID
    /// </summary>
    public long Uid { get; set; }

    /// <summary>
    /// Owner login
    /// </summary>
    public string Login { get; set; } = null!;

    /// <summary>
    /// Owner name
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Is owner verified
    /// </summary>
    public bool Verified { get; set; }
}