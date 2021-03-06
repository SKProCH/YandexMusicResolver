﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using YandexMusicResolver.Config;
using YandexMusicResolver.Responses;

namespace YandexMusicResolver.Requests {
    internal class YandexRequest {
        private HttpWebRequest? _fullRequest;
        private IYandexProxyHolder? _proxyHolder;
        private IYandexTokenHolder? _tokenHolder;

        public YandexRequest(IYandexProxyHolder? proxyHolder, IYandexTokenHolder? tokenHolder) {
            _tokenHolder = tokenHolder;
            _proxyHolder = proxyHolder;
        }

        public YandexRequest(IYandexProxyTokenHolder? config = null) : this(config, config) { }

        protected string GetQueryString(Dictionary<string, string> query) {
            return string.Join("&", query.Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}"));
        }

        protected virtual void FormRequest(string url, string method = WebRequestMethods.Http.Get,
                                           Dictionary<string, string>? query = null, List<KeyValuePair<string, string>>? headers = null,
                                           string? body = null, Action<HttpWebRequest>? afterCreate = null) {
            var queryStr = string.Empty;
            if (query != null && query.Count > 0)
                queryStr = "?" + GetQueryString(query);

            var uri = new Uri($"{url}{queryStr}");
            var request = WebRequest.CreateHttp(uri);
            afterCreate?.Invoke(request);
            request.Method = method;

            if (_proxyHolder?.YandexProxy != null) request.Proxy = _proxyHolder.YandexProxy;

            if (headers != null && headers.Count > 0)
                foreach (var header in headers)
                    request.Headers.Add(header.Key, header.Value);

            TryAddHeader("User-Agent", "Yandex-Music-API");
            TryAddHeader("X-Yandex-Music-Client", "WindowsPhone/3.20");
            if (_tokenHolder?.YandexToken != null) TryAddHeader("Authorization", "OAuth " + _tokenHolder.YandexToken);

            if (!string.IsNullOrEmpty(body)) {
                byte[] bytes = Encoding.UTF8.GetBytes(body);
                Stream s = request.GetRequestStream();
                s.Write(bytes, 0, bytes.Length);

                request.ContentLength = bytes.Length;
            }

            request.KeepAlive = true;
            request.Headers[HttpRequestHeader.AcceptCharset] = Encoding.UTF8.WebName;
            request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip";
            request.AutomaticDecompression = DecompressionMethods.GZip;

            _fullRequest = request;

            void TryAddHeader(string name, string value) {
                if (request.Headers[name] == null) {
                    request.Headers.Add(name, value);
                }
            }
        }

        public async Task EnsureOk() {
            var httpWebResponse = await GetResponseAsync();
            if (httpWebResponse.StatusCode != HttpStatusCode.OK) {
                throw new HttpRequestException("Invalid status code: " + httpWebResponse.StatusCode);
            }
        }

        public async Task<HttpWebResponse> GetResponseAsync() {
            if (_fullRequest != null)
                return (HttpWebResponse) await _fullRequest.GetResponseAsync();
            throw new NullReferenceException("Create request before getting response");
        }

        public async Task<T> GetResponseAsync<T>() {
            var content = await GetResponseBodyAsync();
            YandexApiResponse<T> yandexApiResponse;
            try {
                yandexApiResponse = JsonConvert.DeserializeObject<YandexApiResponse<T>>(content);
            }
            catch (Exception e) {
                throw new Exception("Couldn't get valid API response.", e);
            }

            if (yandexApiResponse.Result != null) return yandexApiResponse.Result;
            if (yandexApiResponse.Error != null) throw new YandexApiResponseException("Couldn't get API response result.", yandexApiResponse.Error);
            throw new Exception("Couldn't get API response result.");
        }

        public async Task<string> GetResponseBodyAsync() {
            var response = await GetResponseAsync();
            using var streamReader = new StreamReader(response.GetResponseStream()!);
            return await streamReader.ReadToEndAsync();
        }
    }
}