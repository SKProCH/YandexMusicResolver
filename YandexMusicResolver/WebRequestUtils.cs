using System.Net;
using System.Threading.Tasks;

namespace YandexMusicResolver {
    internal static class WebRequestUtils {
        public static Task<HttpWebResponse> ExecuteGet(string url, string? token = null, bool isEncoded = false) {
            var webRequest = (HttpWebRequest)WebRequest.Create(url!);
            webRequest.Headers.Add("User-Agent", "Yandex-Music-API");
            webRequest.Headers.Add("X-Yandex-Music-Client", "WindowsPhone/3.20");
            if (token != null) {
                webRequest.Headers.Add("Authorization", "OAuth " + token);
            }

            if (isEncoded) {
                webRequest.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            }

            return Task.Run(async () => (HttpWebResponse) await webRequest.GetResponseAsync());
        }
    }
}