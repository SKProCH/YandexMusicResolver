using System.Net;
using Newtonsoft.Json;

namespace YandexMusicResolver.Config {
    public interface IYandexProxyHolder {
        [JsonIgnore] IWebProxy? YandexProxy { get; set; }
    }
}