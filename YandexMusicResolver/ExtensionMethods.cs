using System;
using System.Security.Authentication;
using System.Threading.Tasks;
using YandexMusicResolver.Config;

namespace YandexMusicResolver {
    /// <summary>
    /// Contains extension methods from YandexMusicResolver
    /// </summary>
    public static class YandexExtensionMethods {
        /// <summary>
        /// Try perform authorization
        /// </summary>
        /// <param name="yandexConfig"></param>
        /// <param name="allowRunWithoutAuth">If false will throw error if we cant authorize</param>
        /// <returns>Task represent current async operation</returns>
        /// <exception cref="AuthenticationException">Will be thrown if we cant authorize and <see cref="allowRunWithoutAuth"/> is false</exception>
        public static async Task AuthorizeAsync(this IYandexConfig yandexConfig, bool allowRunWithoutAuth = true) {
            if (yandexConfig.YandexToken != null)
                if (await YandexMusicAuth.ValidateTokenAsync(yandexConfig.YandexToken, yandexConfig))
                    return;

            if (yandexConfig.YandexLogin == null || yandexConfig.YandexPassword == null) {
                if (allowRunWithoutAuth) return;
                throw new AuthenticationException("Unable to obtain token. Credentials are null.");
            }

            try {
                yandexConfig.YandexToken = await YandexMusicAuth.LoginAsync(yandexConfig.YandexLogin, yandexConfig.YandexPassword, yandexConfig);
                yandexConfig.Save();
            }
            catch (Exception e) {
                if (allowRunWithoutAuth) return;
                throw new AuthenticationException("Unable to obtain token. Credentials are wrong.", e);
            }
        }
    }
}