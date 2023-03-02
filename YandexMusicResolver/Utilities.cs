using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace YandexMusicResolver {
    internal class Utilities {
        public static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new() { WriteIndented = true, PropertyNameCaseInsensitive = true };
        public static string CreateMd5(string input) {
            // Use input string to calculate MD5 hash
            using (MD5 md5 = MD5.Create()) {
                byte[] inputBytes = Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();
                foreach (var t in hashBytes) {
                    sb.Append(t.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
