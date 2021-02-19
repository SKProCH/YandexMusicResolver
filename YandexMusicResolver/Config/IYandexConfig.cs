using System;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace YandexMusicResolver.Config {
    /// <summary>
    /// Represents yandex config
    /// </summary>
    public interface IYandexConfig : IYandexProxyTokenHolder {
        /// <summary>
        /// Load config. This method can be called multiple times
        /// </summary>
        void Load();

        /// <summary>
        /// Save config
        /// </summary>
        void Save();

        /// <summary>
        /// Login for Yandex account
        /// </summary>
        /// <remarks>If specified, will be used with a password to get a token if there are problems with the current one</remarks>
        string? YandexLogin { get; set; }

        /// <summary>
        /// Password for Yandex account
        /// </summary>
        /// <remarks>If specified, will be used with a password to get a token if there are problems with the current one</remarks>
        string? YandexPassword { get; set; }

        /// <summary>
        /// Try perform authorization
        /// </summary>
        /// <param name="allowRunWithoutAuth">If false will throw error if we cant authorize</param>
        /// <returns>Task represent current async operation</returns>
        /// <exception cref="AuthenticationException">Will be thrown if we cant authorize and <see cref="allowRunWithoutAuth"/> is false</exception>
        [Obsolete("Use extension method.\n" +
                  "Will be removed in next major version")]
        public async Task AuthorizeAsync(bool allowRunWithoutAuth = true) {
            if (YandexToken != null)
                if (await YandexMusicAuth.ValidateTokenAsync(YandexToken, this))
                    return;

            if (YandexLogin == null || YandexPassword == null) {
                if (allowRunWithoutAuth) return;
                throw new AuthenticationException("Unable to obtain token. Credentials are null.");
            }

            try {
                YandexToken = await YandexMusicAuth.LoginAsync(YandexLogin, YandexPassword, this);
                Save();
            }
            catch (Exception e) {
                if (allowRunWithoutAuth) return;
                throw new AuthenticationException("Unable to obtain token. Credentials are wrong.", e);
            }
        }
    }
}