using System.Security.Authentication;
using Xunit;

namespace YandexMusicResolver.Tests {
    public class YandexMusicAuthTest : YandexTestBase {
        [Fact]
        public void AuthFailure() {
            Assert.ThrowsAsync<InvalidCredentialException>(async () => {
                await YandexMusicAuth.LoginAsync("Invalid", "Invalid", Config);
            });
        }
    }
}