namespace PdfTools.Net
{
    public interface IHttpResponseMessage
    {
        IHttpContent Content { get; }
    }
}