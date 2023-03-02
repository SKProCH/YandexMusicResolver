namespace YandexMusicResolver.Responses {
    internal class MetaAuthResponse {
        public string AccessToken { get; set; } = null!;

        public long ExpiresIn { get; set; }

        public string TokenType { get; set; } = null!;

        public long Uid { get; set; }
    }
}
