﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Web;
using YandexMusicResolver.Config;
using YandexMusicResolver.Responses;

namespace YandexMusicResolver.Requests {
    internal class YandexAuthRequest : YandexRequest {
        private const string ClientId = "23cabbbdc6cd418abb4b39c32c41195d";
        private const string ClientSecret = "53bc75238f0c4d08a118e51fe9203300";
        public YandexAuthRequest(IYandexProxyHolder? proxyHolder) : base(proxyHolder, null) { }

        public YandexAuthRequest Create(string login, string password) {
            var body = new Dictionary<string, string> {
                { "grant_type", "password" },
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
                { "username", login },
                { "password", password },
            };

            FormRequest("https://oauth.yandex.ru/token", WebRequestMethods.Http.Post,
                body: string.Join("&", body.Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value)}")),
                afterCreate: request => request.ContentType = "application/x-www-form-urlencoded");

            return this;
        }

        public async Task<MetaAuthResponse> ParseResponseAsync() {
            var httpWebResponse = await GetResponseAsync();
            if (httpWebResponse.StatusCode == HttpStatusCode.BadRequest) {
                throw new InvalidCredentialException("Failed to authorize with provided login and password");
            }

            return await GetResponseAsync<MetaAuthResponse>(httpWebResponse);
        }
    }
}
