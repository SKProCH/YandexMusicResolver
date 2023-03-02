namespace YandexMusicResolver.Responses {
    /// <summary>
    /// Represents error that returned from Yandex Music
    /// </summary>
    public class MetaError {
        /// <summary>
        /// Error name
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; set; } = null!;
    }
}
