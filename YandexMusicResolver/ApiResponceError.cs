using System;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver {
    /// <summary>
    /// Represents errors that returned from yandex api.
    /// </summary>
    [Serializable]
    public class YandexApiResponseException : Exception {
        /// <summary>
        /// Contains info about error from yandex api
        /// </summary>
        public MetaError ApiMetaError { get; private set; }

        /// <inheritdoc />
        public YandexApiResponseException(MetaError apiMetaError) {
            ApiMetaError = apiMetaError;
        }

        /// <inheritdoc />
        public YandexApiResponseException(string message, MetaError apiMetaError) : base(message) {
            ApiMetaError = apiMetaError;
        }
    }
}