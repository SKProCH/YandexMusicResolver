using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace YandexMusicResolver.Config {
    public class FileYandexConfig : IYandexConfig {
        private string? _filePath;

        public FileYandexConfig(string? filePath = null) {
            _filePath = filePath ?? "YandexConfig.json";
        }

        public virtual void Load() {
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
        }

        public virtual void Save() {
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public string? YandexLogin { get; set; }
        public string? YandexPassword { get; set; }
        public string? YandexToken { get; set; }
        public string? YandexProxyAddress { get; set; }

        [JsonIgnore]
        public IWebProxy? YandexProxy { get; set; }
    }
}