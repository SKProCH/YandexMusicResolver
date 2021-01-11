using System;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver {
    [Serializable]
    public class YandexApiResponseException : Exception {
        public MetaError ApiMetaError { get; private set; }

        public YandexApiResponseException(MetaError apiMetaError) {
            ApiMetaError = apiMetaError;
        }
        public YandexApiResponseException(string message, MetaError apiMetaError) : base(message) {
            ApiMetaError = apiMetaError;
        }
    }
}