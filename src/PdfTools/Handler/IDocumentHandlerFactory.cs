using System;

namespace PdfTools.Handler
{
    public interface IDocumentHandlerFactory
    {
        IDocumentHandler CreateFromFile(string filepath);
        IDocumentHandler Download(string url);
        IDocumentHandler CreateFromMarkdown(string markdownFile);
        IDocumentHandler CreateEmpty();
    }
}