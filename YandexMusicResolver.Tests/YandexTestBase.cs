using System.IO;
using YandexMusicResolver.Config;

namespace YandexMusicResolver.Tests {
    public class YandexTestBase {
        public IYandexConfig Config;
        public YandexMusicMainResolver MainResolver;

        public YandexTestBase() {
            if (File.Exists("TestData.json")) {
                Config = new FileYandexConfig("TestData.json");
            }
            else {
                Config = new EnvironmentConfig();
            }

            MainResolver = new YandexMusicMainResolver(Config);
        }
    }
}