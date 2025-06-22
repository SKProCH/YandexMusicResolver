using System;
using System.Net;
using YandexMusicResolver.Responses;

namespace YandexMusicResolver;

/// <summary>
/// Represents errors that returned from yandex api.
/// </summary>
[Serializable]
public class YandexApiResponseException : Exception {
    /// <summary>
    /// Contains info about error from yandex api
    /// </summary>
    public MetaError? ApiMetaError { get; private set; }

    /// <summary>
    /// Request status code
    /// </summary>
    public HttpStatusCode HttpStatusCode { get; }

    /// <inheritdoc />
    public YandexApiResponseException(MetaError? apiMetaError, HttpStatusCode httpStatusCode)
        : base($"Couldn't get YandexMusic API response result due to: {apiMetaError?.Message}\n" +
               $"Status code: {httpStatusCode}") {
        ApiMetaError = apiMetaError;
        HttpStatusCode = httpStatusCode;
    }
}
