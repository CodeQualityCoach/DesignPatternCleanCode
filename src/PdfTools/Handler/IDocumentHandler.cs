using System;

namespace PdfTools.Handler
{
    public interface IDocumentHandler : IDisposable
    {
        void AddOverlayImage(string url);
        void Append(string[] fileNames);
        void SaveAs(string destFile);
    }
}