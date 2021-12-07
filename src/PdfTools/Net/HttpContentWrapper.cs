using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PdfTools.Net
{
    public class HttpContentWrapper : IHttpContent, IDisposable
    {
        private readonly HttpContent _httpContent;

        public HttpContentWrapper(HttpContent httpContent)
        {
            _httpContent = httpContent;
        }

        public void Dispose()
        {
            _httpContent?.Dispose();
        }

        // finally we have an abstraction which is based on a .NET type. 
        public Task<byte[]> ReadAsByteArrayAsync()
        {
            return _httpContent.ReadAsByteArrayAsync();
        }
    }
}