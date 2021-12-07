using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

namespace PdfTools.Net
{
    [ExcludeFromCodeCoverage] // This class can be excluded from coverage. We extracted this code because it is not testable (no injectable behaviour).
    public class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient _client;

        // the constructor created the http client which is used. So in case the wrapper is reused, the HttpClient is
        // reused as well. In this case, it preserves he old behaviour in case something in cached by the wrapped class.
        public HttpClientWrapper()
        {
            _client = new HttpClient();
        }

        // It is important that there is a 1:1 mapping of the methods and signatures. A "static class wrapper" should not 
        // contain any logic i.e. parameter validation or transformation.
        public Task<IHttpResponseMessage> GetAsync(string url)
        {
            return Task.FromResult(new HttpResponseMessageWrapper(_client.GetAsync(url).Result) as IHttpResponseMessage);
        }
    }
}