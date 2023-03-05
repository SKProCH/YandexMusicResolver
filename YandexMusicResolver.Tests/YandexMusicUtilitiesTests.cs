using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using Moq.Contrib.HttpClient;
using Xunit;
using YandexMusicResolver.Config;

namespace YandexMusicResolver.Tests {
    public class YandexMusicUtilitiesTests {
        [Fact]
        public async Task PerformYMusicRequestAsync_ShouldCallTokenVerifyIfUnauthorized() {
            var autoMocker = new AutoMocker();

            var credProviderMock = autoMocker.GetMock<IYandexCredentialsProvider>();
            credProviderMock.Setup(provider => provider.GetTokenAsync())
                .ReturnsAsync(() => null);


            var httpClientMock = autoMocker.GetMock<HttpMessageHandler>();
            httpClientMock.SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.Unauthorized);

            var httpClient = httpClientMock.CreateClient();
            await httpClient.PerformYMusicRequestAsync(autoMocker.Get<IYandexCredentialsProvider>(), "https://localhost");
            
            
            credProviderMock.Verify(provider => provider.GetTokenAsync(), Times.Once);
            credProviderMock.Verify(provider => provider.ValidateOrRetrieveTokenAsync(), Times.Once);
            
            httpClientMock.VerifyAnyRequest(Times.Exactly(2));
        }
    }
}