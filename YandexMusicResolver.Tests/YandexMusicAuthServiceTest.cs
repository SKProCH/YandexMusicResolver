using System.Net.Http;
using System.Security.Authentication;
using Xunit;

namespace YandexMusicResolver.Tests {
    public class YandexMusicAuthServiceTest : YandexTestBase {
        [Fact]
        public async System.Threading.Tasks.Task AuthFailure() {
            await Assert.ThrowsAsync<InvalidCredentialException>(async () => {
                await new YandexMusicAuthService(new HttpClient()).LoginAsync("Invalid", "Invalid");
            });
        }
    }
}