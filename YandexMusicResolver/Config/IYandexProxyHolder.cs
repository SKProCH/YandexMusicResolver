namespace YandexMusicResolver.Config {
    /// <summary>
    /// Represents entity which can store Yandex token to use it in requests
    /// </summary>
    public interface IYandexTokenHolder {
        /// <summary>
        /// 
        /// </summary>
        string? YandexToken { get; set; }
    }
}