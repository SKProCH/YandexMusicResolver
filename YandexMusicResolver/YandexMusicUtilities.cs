using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using YandexMusicResolver.Config;
using YandexMusicResolver.Responses;

[assembly: InternalsVisibleTo("YandexMusicResolver.Tests")]

namespace YandexMusicResolver;

/// <summary>
/// Contains some utilities for Yandex Music library
/// </summary>
public static class YandexMusicUtilities {
    /// <summary>
    /// Name for resolving <see cref="HttpClient"/> from <see cref="IHttpClientFactory.CreateClient"/>
    /// </summary>
    public const string HttpClientName = "YandexMusic";

    internal static readonly JsonSerializerOptions DefaultJsonSerializerOptions =
        new() { WriteIndented = true, PropertyNameCaseInsensitive = true };

    internal static string CreateMd5(string input) {
        // Use input string to calculate MD5 hash
        using var md5 = MD5.Create();
        var inputBytes = Encoding.ASCII.GetBytes(input);
        var hashBytes = md5.ComputeHash(inputBytes);

        // Convert the byte array to hexadecimal string
        var sb = new StringBuilder();
        foreach (var t in hashBytes) {
            sb.Append(t.ToString("X2"));
        }

        return sb.ToString();
    }

    internal static HttpClient GetYMusicHttpClient(this IHttpClientFactory factory)
        => factory.CreateClient(HttpClientName);

    internal static async Task<T> PerformYMusicRequestAsync<T>(this HttpClient httpClient,
        IYandexCredentialsProvider? credentialsProvider,
        string url, HttpContent? httpContent = null, HttpMethod? method = null) {
        var response = await PerformYMusicRequestAsync(httpClient, credentialsProvider, url, httpContent, method);
        return await ParseYMusicResponseAsync<T>(response);
    }

    internal static async Task<HttpResponseMessage> PerformYMusicRequestAsync(this HttpClient httpClient,
        IYandexCredentialsProvider? credentialsProvider,
        string url, HttpContent? httpContent = null, HttpMethod? method = null) {
        var token = credentialsProvider == null ? null : await credentialsProvider.GetTokenAsync();
        var httpRequestMessage = FormHttpRequestMessage(url, token, httpContent, method);

        var response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);

        // Retry if unauthorized
        if (response.StatusCode == HttpStatusCode.Unauthorized && credentialsProvider != null) {
            token = await credentialsProvider.ValidateOrRetrieveTokenAsync();
            httpRequestMessage = FormHttpRequestMessage(url, token, httpContent, method);

            response = await httpClient.SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead);
        }

        return response;
    }

    internal static async Task<T> ParseYMusicResponseAsync<T>(this HttpResponseMessage response) {
        var responseStream = await response.Content.ReadAsStreamAsync();
        if (response.IsSuccessStatusCode) {
            var yandexApiResponse =
                await JsonSerializer.DeserializeAsync<YandexApiResponse<T>>(responseStream,
                    DefaultJsonSerializerOptions)
                ?? throw new InvalidOperationException();
            if (yandexApiResponse.Result != null) return yandexApiResponse.Result;
            throw new Exception("YandexMusic API returned success code and missing result");
        }

        using var streamReader = new StreamReader(responseStream);
        var errorString = await streamReader.ReadToEndAsync();

        YandexApiResponse<MetaError> errorResult;
        try {
            errorResult = JsonSerializer.Deserialize<YandexApiResponse<MetaError>>(errorString, DefaultJsonSerializerOptions)
                          ?? throw new InvalidOperationException();
        }
        catch (Exception) {
            var apiMetaError = new MetaError { Name = "Unparseable error from Yandex Music API", Message = errorString };
            throw new YandexApiResponseException(apiMetaError, response.StatusCode);
        }
        throw new YandexApiResponseException(errorResult.Result!, response.StatusCode);
    }

    private static HttpRequestMessage FormHttpRequestMessage(string url, string? token, HttpContent? httpContent,
        HttpMethod? method) {
        var httpRequestMessage = new HttpRequestMessage(method ?? HttpMethod.Get, url);
        httpRequestMessage.Headers.Add("User-Agent", "Yandex-Music-API");
        httpRequestMessage.Headers.Add("X-Yandex-Music-Client", "WindowsPhone/3.20");

        if (token != null) {
            httpRequestMessage.Headers.Add("Authorization", "OAuth " + token);
        }

        if (httpContent != null) {
            httpRequestMessage.Content = httpContent;
        }

        return httpRequestMessage;
    }
}
