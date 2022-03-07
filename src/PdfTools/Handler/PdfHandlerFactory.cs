using System.IO;
using System.IO.Abstractions;
using FSharp.Markdown;
using FSharp.Markdown.Pdf;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfTools.Net;

namespace PdfTools.Handler
{
    /// <summary>
    /// This created 
    /// </summary>
    public class PdfHandlerFactory : IDocumentHandlerFactory
    {
        private readonly IHttpClient _httpClient;
        private readonly IFileSystem _fileSystem;

        public PdfHandlerFactory(IFileSystem fileSystem = null, IHttpClient httpClient = null)
        {
            _httpClient = httpClient ?? new HttpClientWrapper();
            _fileSystem = fileSystem ?? new FileSystem();
        }

        public IDocumentHandler CreateFromFile(string filepath)
        {
            var tempFile = _fileSystem.Path.GetTempFileName();
            _fileSystem.File.Copy(filepath, tempFile, true);
            return new PdfHandler(tempFile);
        }

        public IDocumentHandler Download(string url)
        {
            var tempFile = _fileSystem.Path.GetTempFileName();

            // if you start a 1:1 mapping from the handler to the factory, you only need to change the initial call.
            var response = _httpClient.GetAsync(url).Result;
            var pdf = response.Content.ReadAsByteArrayAsync().Result;

            var tmpTempFile = _fileSystem.Path.GetTempFileName();
            _fileSystem.File.WriteAllBytes(tmpTempFile, pdf);
            return new PdfHandler(tempFile);
        }

        public IDocumentHandler CreateFromMarkdown(string markdownFile)
        {
            var tempFile = _fileSystem.Path.GetTempFileName();
            var mdText = File.ReadAllText(markdownFile);
            var mdDoc = Markdown.Parse(mdText);

            MarkdownPdf.Write(mdDoc, tempFile);
            return new PdfHandler(tempFile);
        }

        public IDocumentHandler CreateEmpty()
        {
            var tempFile = _fileSystem.Path.GetTempFileName();
            return new PdfHandler(tempFile);
        }
    }
}