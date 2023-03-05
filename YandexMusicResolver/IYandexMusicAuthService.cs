using System.Security.Authentication;
using System.Threading.Tasks;
using YandexMusicResolver.Responses;

namespace YandexMusicResolver;

public interface IYandexMusicAuthService {
    /// <summary>
    /// Validates token
    /// </summary>
    /// <param name="token">Token to validate</param>
    /// <returns>True if token valid</returns>
    Task<bool> ValidateTokenAsync(string token);
    /// <summary>
    /// Attempt to authorise
    /// </summary>
    /// <param name="login">Login from Yandex account</param>
    /// <param name="password">Password from Yandex account</param>
    /// <exception cref="InvalidCredentialException">Throws when failed to authorize with provided login and password</exception>
    /// <returns>Token</returns>
    Task<YandexAuthResponse> LoginAsync(string login, string password);
}
