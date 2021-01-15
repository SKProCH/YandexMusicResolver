using System.Net;

namespace YandexMusicResolver.Config {
    /// <summary>
    /// Represents <see cref="IYandexConfig"/> implementation placeholder
    /// </summary>
    public class EmptyYandexConfig : IYandexConfig {
        /// <inheritdoc />
        public void Load() { }

        /// <inheritdoc />
        public void Save() { }

        /// <inheritdoc />
        public string? YandexLogin { get; set; }

        /// <inheritdoc />
        public string? YandexPassword { get; set; }

        /// <inheritdoc />
        public string? YandexToken { get; set; }

        /// <inheritdoc />
        public IWebProxy? YandexProxy { get; set; }
    }
}