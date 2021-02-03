using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace YandexMusicResolver.Config {
    /// <summary>
    /// Represents <see cref="IYandexConfig"/> implementation that stores data in a file
    /// </summary>
    public class FileYandexConfig : IYandexConfig {
        private string _filePath;
        private bool isLoaded;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileYandexConfig"/> class.
        /// </summary>
        /// <param name="filePath">Target file path</param>
        public FileYandexConfig(string? filePath = null) {
            _filePath = filePath ?? "YandexConfig.json";
        }

        /// <inheritdoc />
        public virtual void Load() {
            if (isLoaded) return;
            try {
                if (File.Exists(_filePath)) {
                    var fileYandexConfig = JsonConvert.DeserializeObject<FileYandexConfig>(File.ReadAllText(_filePath));
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

            isLoaded = true;
        }

        /// <inheritdoc />
        public virtual void Save() {
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        /// <inheritdoc />
        public string? YandexLogin { get; set; }

        /// <inheritdoc />
        public string? YandexPassword { get; set; }

        /// <inheritdoc />
        public string? YandexToken { get; set; }

        /// <summary>
        /// Uri to create proxy
        /// </summary>
        public string? YandexProxyAddress { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public IWebProxy? YandexProxy { get; set; }
    }
}