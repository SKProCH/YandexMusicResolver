namespace YandexMusicResolver.Responses {
    internal class YandexApiResponse<T> {
        public T? Result { get; set; }

        public MetaError? Error { get; set; }
    }
}
