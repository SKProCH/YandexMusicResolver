using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace YandexMusicResolver.Tests;

public class DirectUrlLoaderTest : YandexTestBase{
    private readonly ITestOutputHelper _output;
    public DirectUrlLoaderTest(ITestOutputHelper output) {
        _output = output;
    }

    [Theory]
    [InlineData(135525714)]
    [InlineData(37637150)]
    public async Task DirectLoadTest(long trackId) {
        var result = await MainResolver.DirectUrlLoader.GetDirectUrl(trackId, "mp3");
        var webRequest = await new HttpClient().GetAsync(result, HttpCompletionOption.ResponseHeadersRead);
        Assert.Equal(HttpStatusCode.OK, webRequest.StatusCode);
        _output.WriteLine(result);
    }
}