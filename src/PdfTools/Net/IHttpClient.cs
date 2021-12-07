using System.Threading.Tasks;

namespace PdfTools.Net
{
    public interface IHttpClient
    {
        // If you solve this 100% correctly, the HttpResponseMessage message must be wrapped as well and provided as an interface.
        Task<IHttpResponseMessage> GetAsync(string url);
    }
}
