#nullable enable
using System;
using System.Net;
using YandexMusicResolver.Config;

namespace YandexMusicResolver.Tests {
    public class EnvironmentConfig : IYandexConfig {
        private bool isLoaded;
        public void Load() {
            if (isLoaded) return;
            YandexLogin = Environment.GetEnvironmentVariable("YandexLogin");
            YandexPassword = Environment.GetEnvironmentVariable("YandexPassword");
            YandexToken = Environment.GetEnvironmentVariable("YandexToken");
            
            var proxyUrl = Environment.GetEnvironmentVariable("YandexProxy");
            if (proxyUrl != null) {
                YandexProxy = new WebProxy(proxyUrl);
            }

            isLoaded = true;
        }

        public void Save() { }

        public string? YandexLogin { get; set; }
        public string? YandexPassword { get; set; }
        public IWebProxy? YandexProxy { get; set; }
        public string? YandexToken { get; set; }
    }
}