using System;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace YandexMusicResolver.Config;

/// <summary>
/// Default implementation for credentials provider
/// </summary>
public class YandexCredentialsProvider : IYandexCredentialsProvider {
    private readonly IYandexMusicAuthService _yandexMusicAuthService;
    private string? _login;
    private string? _password;

    private string? _token;
    private DateTime? _tokenExpirationDate;
    
    /// <summary>
    /// Initializes a new <see cref="YandexCredentialsProvider"/> with login, password, token and <see cref="IYandexMusicAuthService"/>
    /// </summary>
    public YandexCredentialsProvider(IYandexMusicAuthService yandexMusicAuthService, string login, string password, string? token = null) {
        _yandexMusicAuthService = yandexMusicAuthService;
        _login = login;
        _password = password;
        _token = token;
        AllowAnonymous = false;
    }
    
    /// <summary>
    /// Initializes a new <see cref="YandexCredentialsProvider"/>
    /// </summary>
    public YandexCredentialsProvider(IYandexMusicAuthService yandexMusicAuthService, string token, bool allowAnonymous) {
        _yandexMusicAuthService = yandexMusicAuthService;
        _token = token;
        AllowAnonymous = allowAnonymous;
    }
    
    /// <summary>
    /// Initializes a new <see cref="YandexCredentialsProvider"/> with anonymizes access
    /// </summary>
    public YandexCredentialsProvider(IYandexMusicAuthService yandexMusicAuthService, YandexCredentials credentials) {
        _yandexMusicAuthService = yandexMusicAuthService;
        _login = credentials.Login;
        _password = credentials.Login;
        _token = credentials.Token;
        AllowAnonymous = credentials.AllowAnonymous;
    }

    /// <inheritdoc />
    public async Task<string?> GetTokenAsync() {
        if (_token == null) {
            if (_login == null || _password == null) {
                if (AllowAnonymous) {
                    return null;
                }

                throw new InvalidCredentialException("Anonymous access disabled, but no login or password provided");
            }
            
            return await PerformLoginAsync();
        }

        if (_tokenExpirationDate.GetValueOrDefault(DateTime.MaxValue) <= DateTime.UtcNow) {
            return await PerformLoginAsync();
        }

        return _token;
    }
    
    /// <inheritdoc />
    public async Task<string?> ValidateOrRetrieveTokenAsync() {
        if (_token != null && _tokenExpirationDate.GetValueOrDefault(DateTime.MaxValue) <= DateTime.UtcNow) {
            if (await _yandexMusicAuthService.ValidateTokenAsync(_token))
                return _token;
        }
        
        if (_login == null || _password == null) {
            if (AllowAnonymous) {
                return null;
            }

            throw new InvalidCredentialException("Anonymous access disabled, but no login or password provided");
        }
            
        return await PerformLoginAsync();
    }

    /// <inheritdoc />
    public event EventHandler<YandexTokenChangedEventArgs>? TokenChanged;

    /// <summary>
    /// Allow requests without token
    /// </summary>
    public bool AllowAnonymous { get; }

    private async Task<string> PerformLoginAsync() {
        var yandexAuthResponse = await _yandexMusicAuthService.LoginAsync(_login!, _password!);
        _token = yandexAuthResponse.AccessToken;
        _tokenExpirationDate = yandexAuthResponse.ExpiresInDateTime.UtcDateTime;
        OnTokenChanged(yandexAuthResponse.AccessToken);
        return yandexAuthResponse.AccessToken;
    }
    
    /// <summary>
    /// Raises <see cref="TokenChanged"/>
    /// </summary>
    /// <param name="token">Token</param>
    protected virtual void OnTokenChanged(string token) {
        TokenChanged?.Invoke(this, new YandexTokenChangedEventArgs(token));
    }
}
