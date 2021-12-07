using System.Net.Http;

namespace PdfTools.Net
{
    public class HttpResponseMessageWrapper : IHttpResponseMessage
    {
        private readonly HttpResponseMessage _responseMessage;

        public HttpResponseMessageWrapper(HttpResponseMessage responseMessage)
        {
            _responseMessage = responseMessage;
        }

        public IHttpContent Content => new HttpContentWrapper( _responseMessage.Content);
    }
}