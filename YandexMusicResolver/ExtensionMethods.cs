using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YandexMusicResolver.Responces;

namespace YandexMusicResolver {
    internal static class ExtensionMethods {
        public static async Task<string> GetContent(this Task<HttpWebResponse> response) {
            return GetContent(await response);
        }

        public static string GetContent(this HttpWebResponse response) {
            response.EnsureOk();
            using var streamReader = new StreamReader(response.GetResponseStream()!);
            return streamReader.ReadToEnd();
        }

        public static async Task<T> Parse<T>(this Task<HttpWebResponse> taskResponse) {
            return Parse<T>(await taskResponse);
        }
        
        public static T Parse<T>(this HttpWebResponse taskResponse) {
            var content = taskResponse.GetContent();
            YandexApiResponse<T> yandexApiResponse;
            try {
                yandexApiResponse = JsonConvert.DeserializeObject<YandexApiResponse<T>>(content);
            }
            catch (Exception e) {
                throw new Exception("Couldn't get valid API response.", e);
            }

            if (yandexApiResponse.Result == null) {
                if (yandexApiResponse.Error != null) throw new YandexApiResponseException("Couldn't get API response result.", yandexApiResponse.Error);
                throw new Exception("Couldn't get API response result.");
            }

            return yandexApiResponse.Result;
        }

        public static async Task EnsureOk(this Task<HttpWebResponse> responseTask) {
            EnsureOk(await responseTask);
        }

        public static void EnsureOk(this HttpWebResponse webResponse) {
            if (webResponse.StatusCode != HttpStatusCode.OK) {
                throw new HttpRequestException("Invalid status code: " + webResponse.StatusCode);
            }
        }
    }
}