using System.Net;
using System.Security.Authentication;
using Xunit;
using Xunit.Abstractions;

namespace YandexMusicResolver.Tests {
    public class YandexMusicAuthTest : YandexTestBase{
        private readonly ITestOutputHelper _output;
        public YandexMusicAuthTest(ITestOutputHelper output) {
            _output = output;
        }

        [Fact]
        public void AuthFailure() {
            Assert.ThrowsAsync<InvalidCredentialException>(async () => {
                await YandexMusicAuth.LoginAsync("Invalid", "Invalid", Config);
            });
        }
    }
}