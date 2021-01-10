using System.Net;
using Xunit;
using Xunit.Abstractions;

namespace YandexMusicResolver.Tests {
    public class YandexMusicDirectUrlLoaderTest : YandexTestBase{
        private readonly ITestOutputHelper _output;
        public YandexMusicDirectUrlLoaderTest(ITestOutputHelper output) {
            _output = output;
        }

        [Theory]
        [InlineData("43413021")]
        [InlineData("37637150")]
        public void DirectLoadTest(string trackId) {
            var result = MainResolver.DirectUrlLoader.GetDirectUrl(trackId, "mp3").GetAwaiter().GetResult();
            var webRequest = (HttpWebResponse)WebRequest.Create(result).GetResponse();
            Assert.Equal(HttpStatusCode.OK, webRequest.StatusCode);
            _output.WriteLine(result);
        }
    }
}