using System.Net;
using System.Text.Json.Serialization;

namespace YandexMusicResolver.Config {
    /// <summary>
    /// Represents entity that must contains proxy
    /// </summary>
    public interface IYandexProxyHolder {
        /// <summary>
        /// Gets or sets proxy to use with requests
        /// </summary>
        [JsonIgnore]
        IWebProxy? YandexProxy { get; set; }
    }
}
