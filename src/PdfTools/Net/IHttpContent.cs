using System.Threading.Tasks;

namespace PdfTools.Net
{
    public interface IHttpContent
    {
        Task<byte[]> ReadAsByteArrayAsync();
    }
}