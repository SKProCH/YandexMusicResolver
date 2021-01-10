using System.Net;
using Newtonsoft.Json;

namespace YandexMusicResolver.Config {
    public interface IYandexConfig {
        void Load();
        void Save();
        
        string? YandexLogin { get; set; }
        string? YandexPassword { get; set; }
        
        string? YandexToken { get; set; }
        
        [JsonIgnore] IWebProxy? YandexProxy { get; set; }
    }
}