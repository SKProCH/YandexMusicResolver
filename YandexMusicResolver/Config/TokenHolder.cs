namespace YandexMusicResolver.Config {
    internal class TokenHolder : IYandexTokenHolder {
        public TokenHolder(string? yandexToken) {
            YandexToken = yandexToken;
        }

        public string? YandexToken { get; set; }
    }
}