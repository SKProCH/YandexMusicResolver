namespace YandexMusicResolver.Config {
    /// <summary>
    /// Represents entity what must contain proxy and token
    /// </summary>
    public interface IYandexProxyTokenHolder : IYandexProxyHolder, IYandexTokenHolder { }
}