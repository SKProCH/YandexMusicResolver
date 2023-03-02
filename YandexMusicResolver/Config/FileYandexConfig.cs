using System;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace YandexMusicResolver.Config {
    /// <summary>
    /// Represents <see cref="IYandexConfig"/> implementation that stores data in a file
    /// </summary>
    public class FileYandexConfig : IYandexConfig {
        private string _filePath;
        private bool _isLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileYandexConfig"/> class.
        /// </summary>
        /// <param name="filePath">Target file path</param>
        public FileYandexConfig(string? filePath = null) {
            _filePath = filePath ?? "YandexConfig.json";
        }

        /// <summary>
        /// Uri to create proxy
        /// </summary>
        public string? YandexProxyAddress { get; set; }

        /// <inheritdoc />
        public virtual void Load() {
            if (_isLoaded) return;
            try {
                if (File.Exists(_filePath)) {
                    var fileYandexConfig = JsonSerializer.Deserialize<FileYandexConfig>(File.ReadAllText(_filePath), Utilities.DefaultJsonSerializerOptions)!;
                    YandexLogin = fileYandexConfig.YandexLogin;
                    YandexPassword = fileYandexConfig.YandexPassword;
                    YandexToken = fileYandexConfig.YandexToken;
                    YandexProxyAddress = fileYandexConfig.YandexProxyAddress;
                    if (YandexProxyAddress != null) {
                        YandexProxy = new WebProxy(YandexProxyAddress);
                    }
                }
                else {
                    // Create dummy file
                    Save();
                }
            }
            catch (Exception) {
                // ignored
            }

            _isLoaded = true;
        }

        /// <inheritdoc />
        public virtual void Save() {
            File.WriteAllText(_filePath, JsonSerializer.Serialize(this, Utilities.DefaultJsonSerializerOptions));
        }

        /// <inheritdoc />
        public string? YandexLogin { get; set; }

        /// <inheritdoc />
        public string? YandexPassword { get; set; }

        /// <inheritdoc />
        public string? YandexToken { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public IWebProxy? YandexProxy { get; set; }
    }
}
