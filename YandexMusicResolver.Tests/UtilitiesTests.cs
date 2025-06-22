using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.AutoMock;
using Moq.Contrib.HttpClient;
using Xunit;
using YandexMusicResolver.Config;
using YandexMusicResolver.Responses;

namespace YandexMusicResolver.Tests;

public class UtilitiesTests {
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

    [Fact]
    public void ShouldPropertyRegisterAllServices()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddYandexMusicResolver();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var yandexMusicMainResolver = serviceProvider.GetRequiredService<IYandexMusicMainResolver>();
    }
    
    [Fact]
    public async Task PerformYMusicRequestAsync_ShouldHandleErrors()
    {
        var httpClient = new HttpClient();
        // Unavailable track
        var exception = await Assert.ThrowsAsync<YandexApiResponseException>(() =>
            httpClient.PerformYMusicRequestAsync<List<MetaTrackDownloadInfo>>(
                YandexAnonymousCredentialProvider.Instance,
                "https://api.music.yandex.net/tracks/43413021/download-info"));
        
        Assert.Equal("no-rights", exception.ApiMetaError?.Message);
        Assert.Equal(HttpStatusCode.Forbidden, exception.HttpStatusCode);
    }
}