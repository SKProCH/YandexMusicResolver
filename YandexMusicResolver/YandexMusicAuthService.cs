using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using YandexMusicResolver.Responses;

namespace YandexMusicResolver; 

/// <summary>
/// Service to accessing Yandex Music authorization
/// </summary>
public class YandexMusicAuthService : IYandexMusicAuthService {
    private const string StatusUrl = "https://api.music.yandex.net/account/status";
    private const string AuthUrl = "https://oauth.yandex.ru/token";

    private const string ClientId = "23cabbbdc6cd418abb4b39c32c41195d";
    private const string ClientSecret = "53bc75238f0c4d08a118e51fe9203300";
    
    private HttpClient _httpClient;
    /// <summary>
    /// Create instance of <see cref="YandexMusicAuthService"/>
    /// </summary>
    /// <param name="httpClientFactory">Factory for resolving HttpClient. Client name is <see cref="YandexMusicUtilities.HttpClientName"/></param>
    public YandexMusicAuthService(IHttpClientFactory httpClientFactory) {
        _httpClient = httpClientFactory.GetYMusicHttpClient();
    }

    /// <summary>
    /// Create instance of <see cref="YandexMusicAuthService"/>
    /// </summary>
    /// <param name="httpClient">HttpClient for performing requests. But preferred way is use another ctor and pass <see cref="IHttpClientFactory"/></param>
    public YandexMusicAuthService(HttpClient httpClient) {
        _httpClient = httpClient;
    }

    /// <inheritdoc />
    public async Task<bool> ValidateTokenAsync(string token) {
        var metaAccountResponse = await _httpClient.PerformYMusicRequestAsync<MetaAccountResponse>(null, StatusUrl);
        return !string.IsNullOrEmpty(metaAccountResponse.Account?.Uid);
    }

    /// <inheritdoc />
    public async Task<YandexAuthResponse> LoginAsync(string login, string password) {
        var body = new Dictionary<string, string> {
            { "grant_type", "password" },
            { "client_id", ClientId },
            { "client_secret", ClientSecret },
            { "username", login },
            { "password", password },
        };

        var response = await _httpClient.PerformYMusicRequestAsync(null, AuthUrl,
            new FormUrlEncodedContent(body), HttpMethod.Post);
        
        if (response.StatusCode == HttpStatusCode.BadRequest) {
            throw new InvalidCredentialException("Failed to authorize with provided login and password");
        }

        return await response.ParseYMusicResponseAsync<YandexAuthResponse>();
    }
}
