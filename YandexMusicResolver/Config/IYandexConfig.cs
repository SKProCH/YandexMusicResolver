using System.Net;

namespace YandexMusicResolver.Config {
    public interface IYandexConfig {
        void Load();
        void Save();
        
        string? YandexLogin { get; set; }
        string? YandexPassword { get; set; }
        
        string? YandexToken { get; set; }
        
        IWebProxy? YandexProxy { get; set; }
    }
}