namespace YandexMusicResolver.Requests {
    internal class YandexCustomRequest : YandexRequest {
        public YandexCustomRequest(string? token = null) : base(token) { }

        public YandexCustomRequest Create(string url) {
            FormRequest(url);
            return this;
        }
    }
}