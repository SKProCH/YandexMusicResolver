using System.Net;

namespace YandexMusicResolver.Config {
    public class EmptyYandexConfig : IYandexConfig {
        public void Load() {
            
        }

        public void Save() {
            
        }

        public string? YandexLogin { get; set; }
        public string? YandexPassword { get; set; }
        public string? YandexToken { get; set; }
        public IWebProxy? YandexProxy { get; set; }
    }
}