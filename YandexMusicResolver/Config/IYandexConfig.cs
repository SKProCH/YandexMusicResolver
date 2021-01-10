using System;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace YandexMusicResolver.Config {
    public interface IYandexConfig : IYandexProxyTokenHolder {
        void Load();
        void Save();
        
        string? YandexLogin { get; set; }
        string? YandexPassword { get; set; }

        public async Task AuthorizeAsync(bool allowRunWithoutAuth = true) {
            if (YandexToken != null)
                if (await YandexMusicAuth.CheckToken(YandexToken, this))
                    return;
            
            
            if (YandexLogin == null || YandexPassword == null) {
                if (allowRunWithoutAuth) return;
                throw new AuthenticationException("Unable to obtain token. Credentials are null.");
            }

            try {
                YandexToken = await YandexMusicAuth.GetToken(YandexLogin, YandexPassword, this);
                Save();
            }
            catch (Exception e) {
                if (allowRunWithoutAuth) return;
                throw new AuthenticationException("Unable to obtain token. Credentials are wrong.", e);
            }
        }
    }
}