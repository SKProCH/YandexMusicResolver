using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using YandexMusicResolver.Config;
using YandexMusicResolver.Requests;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver {
    public class YandexMusicAuth {
        private const string AuthPattern = "https://oauth.yandex.ru/token";
        private const string ClientId = "23cabbbdc6cd418abb4b39c32c41195d";
        private const string ClientSecret = "53bc75238f0c4d08a118e51fe9203300";
        public static async Task<bool> CheckToken(string token, IYandexProxyHolder? proxyHolder = null) {
            var metaAccountResponse = await new YandexCustomRequest(proxyHolder, new TokenHolder(token)).Create("https://api.music.yandex.net/account/status").GetResponseAsync<MetaAccountResponse>();
            return !string.IsNullOrEmpty(metaAccountResponse?.Account?.Uid);
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