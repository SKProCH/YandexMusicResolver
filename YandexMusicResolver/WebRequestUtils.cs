using System.Net;
using System.Threading.Tasks;

namespace YandexMusicResolver {
    internal class WebRequestUtils {
        public static Task<HttpWebResponse> ExecuteGet(string url, string? token = null) {
            var webRequest = (HttpWebRequest)WebRequest.Create(WebUtility.UrlEncode(url)!);
            webRequest.Headers.Add("User-Agent", "Yandex-Music-API");
            webRequest.Headers.Add("X-Yandex-Music-Client", "WindowsPhone/3.20");
            if (token != null) {
                webRequest.Headers.Add("Authorization", "OAuth " + token);
            }

            return Task.Run(async () => (HttpWebResponse) await webRequest.GetResponseAsync());
        }
    }
}