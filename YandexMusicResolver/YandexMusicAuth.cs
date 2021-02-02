using System.Threading.Tasks;
using YandexMusicResolver.Config;
using YandexMusicResolver.Requests;
using YandexMusicResolver.Responses;

namespace YandexMusicResolver {
    /// <summary>
    /// Represents a set of methods that serve for authorization in Yandex Music
    /// </summary>
    public static class YandexMusicAuth {
        /// <summary>
        /// Validates token
        /// </summary>
        /// <param name="token">Token to validate</param>
        /// <param name="proxyHolder">Container for proxy, which should be used for request</param>
        /// <returns>True if token valid</returns>
        public static async Task<bool> ValidateTokenAsync(string token, IYandexProxyHolder? proxyHolder = null) {
            var metaAccountResponse = await new YandexCustomRequest(proxyHolder, new TokenHolder(token))
                                           .Create("https://api.music.yandex.net/account/status")
                                           .GetResponseAsync<MetaAccountResponse>();
            return !string.IsNullOrEmpty(metaAccountResponse.Account?.Uid);
        }

        /// <summary>
        /// Attempt to authorise
        /// </summary>
        /// <param name="login">Login from Yandex account</param>
        /// <param name="password">Password from Yandex account</param>
        /// <param name="proxyHolder">Container for proxy, which should be used for request</param>
        /// <returns>Token</returns>
        public static async Task<string> LoginAsync(string login, string password, IYandexProxyHolder? proxyHolder = null) {
            return (await new YandexAuthRequest(proxyHolder).Create(login, password).ParseResponseAsync()).AccessToken;
        }

        /// <summary>
        /// Try to validate token or get new one using login and password
        /// </summary>
        /// <param name="existentToken">Token to validate</param>
        /// <param name="fallbackLogin">Login from Yandex account</param>
        /// <param name="fallbackPassword">Password from Yandex account</param>
        /// <param name="proxyHolder">Container for proxy, which should be used for request</param>
        /// <returns>Valid token, true if this is new token otherwise false</returns>
        public static async Task<string> ValidateOrLoginAsync(string? existentToken, string fallbackLogin, string fallbackPassword,
                                                          IYandexProxyHolder? proxyHolder = null) {
            if (string.IsNullOrWhiteSpace(existentToken) || !await ValidateTokenAsync(existentToken, proxyHolder)) {
                return await LoginAsync(fallbackLogin, fallbackPassword, proxyHolder);
            }

            return existentToken;
        }
    }
}