using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using YandexMusicResolver.Requests;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver {
    public class YandexMusicAuth {
        private const string AuthPattern = "https://oauth.yandex.ru/token";
        private const string ClientId = "23cabbbdc6cd418abb4b39c32c41195d";
        private const string ClientSecret = "53bc75238f0c4d08a118e51fe9203300";
        public static async Task<bool> CheckToken(string token) {
            var metaAccountResponse = await new YandexCustomRequest(token).Create("https://api.music.yandex.net/account/status").GetResponseAsync<MetaAccountResponse>();
            return !string.IsNullOrEmpty(metaAccountResponse?.Account?.Uid);
        }

        public static async Task<string> GetToken(string login, string password) {
            return (await new YandexAuthRequest().Create(login, password).ParseResponseAsync()).AccessToken;
        }

        public static async Task<(string, bool)> GetToken(string existentToken, string fallbackLogin, string fallbackPassword) {
            if (string.IsNullOrWhiteSpace(existentToken) || !await CheckToken(existentToken)) {
                return (await GetToken(fallbackLogin, fallbackPassword), true);
            }

            return (existentToken, false);
        }
    }
}