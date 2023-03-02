namespace YandexMusicResolver.AudioItems {
    /// <summary>
    /// Represent a artist in Yandex Music
    /// </summary>
    public class YandexMusicArtist {
        /// <summary>
        /// Artist ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Artist name
        /// </summary>
        public string Name { get; set; } = null!;
    }
}
