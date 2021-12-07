using System;
using System.Net.Http;

namespace PdfTools.Net
{
    public class HttpResponseMessageWrapper : IHttpResponseMessage, IDisposable
    {
        private readonly HttpResponseMessage _responseMessage;

        public HttpResponseMessageWrapper(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage;
        }

        public IHttpContent Content => new HttpContentWrapper( _responseMessage.Content);

        public void Dispose()
        {
            _responseMessage?.Dispose();
        }
    }
}