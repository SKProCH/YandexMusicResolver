using System.Threading.Tasks;
using YandexMusicResolver.Config;
using YandexMusicResolver.Requests;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver {
    public class YandexMusicAuth {
        public static async Task<bool> CheckToken(string token, IYandexProxyHolder? proxyHolder = null) {
            var metaAccountResponse = await new YandexCustomRequest(proxyHolder, new TokenHolder(token)).Create("https://api.music.yandex.net/account/status").GetResponseAsync<MetaAccountResponse>();
            return !string.IsNullOrEmpty(metaAccountResponse.Account?.Uid);
        }

        public static async Task<string> GetToken(string login, string password, IYandexProxyHolder? proxyHolder = null) {
            return (await new YandexAuthRequest(proxyHolder).Create(login, password).ParseResponseAsync()).AccessToken;
        }

        public static async Task<(string, bool)> GetToken(string? existentToken, string fallbackLogin, string fallbackPassword, IYandexProxyHolder? proxyHolder = null) {
            if (string.IsNullOrWhiteSpace(existentToken) || !await CheckToken(existentToken, proxyHolder)) {
                return (await GetToken(fallbackLogin, fallbackPassword, proxyHolder), true);
            }

            return (existentToken, false);
        }
    }
}