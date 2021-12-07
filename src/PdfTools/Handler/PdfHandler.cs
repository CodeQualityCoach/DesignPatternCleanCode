using System;
using System.Drawing.Imaging;
using System.IO;
using iTextSharp.text.pdf;
using PdfTools.Net;
using PdfTools.Services;
using Image = iTextSharp.text.Image;

namespace PdfTools.Handler
{
    /// <summary>
    /// The PdfHandler manages a pdf file by opening or downloading it. The handler can process the document type "pdf" and
    /// add images to the file. The handler stores a local copy (instead of a memory stream) to keep it as easy as possible.
    /// </summary>
    public class PdfHandler : IDisposable
    {
        private readonly IHttpClient _httpClient;
        private readonly IOverlayImageService _imageGenerator;
        private readonly string _tempFile;

        public PdfHandler(IOverlayImageService imageGenerator = null, IHttpClient httpClient = null)
        {
            // here we use "Zero Impact Injection", which means, the calling class has not changed and functionality
            // has not changed. The code behaves as always, but in testing we will see, that we can inject different behaviour.
            _httpClient = httpClient ?? new HttpClientWrapper();
            _imageGenerator = imageGenerator ?? new QrCoderService();

            _tempFile = Path.GetTempFileName();
        }

        public void Open(string filepath)
        {
            File.Copy(filepath, _tempFile, true);
        }

        public void Download(string url)
        {
            // if you start a 1:1 mapping, you only need to change the initial call.
            var response = _httpClient.GetAsync(url).Result;
            var pdf = response.Content.ReadAsByteArrayAsync().Result;

            var tmpTempFile = Path.GetTempFileName();
            File.WriteAllBytes(tmpTempFile, pdf);

        }

        public void AddOverlayImage(string url)
        {
            using (Stream inputPdfStream = new FileStream(_tempFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream inputImageStream = new MemoryStream())
            using (Stream outputPdfStream = new FileStream(_tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var code = Uri.TryCreate(url, UriKind.Absolute, out var uri)
                    ? _imageGenerator.CreateOverlayImage(uri)
                    : _imageGenerator.CreateOverlayImage(url);

                code.Save(inputImageStream, ImageFormat.Jpeg);
                inputImageStream.Position = 0;

                var reader = new PdfReader(inputPdfStream);
                var stamper = new PdfStamper(reader, outputPdfStream);
                var pdfContentByte = stamper.GetOverContent(1);

                var image = Image.GetInstance(inputImageStream);
                image.SetAbsolutePosition(5, 5);
                pdfContentByte.AddImage(image);
                stamper.Close();
            }
        }

        public void SaveAs(string destFile)
        {
            File.Copy(_tempFile, destFile, true);
        }

        #region Implement IDisposable with finalizer

        // this virtual method is called by finalizer and Dispose() and can be extended in derived classes
        protected virtual void Dispose(bool disposing)
        {
            // is disposing is called explicitly, we need to free managed (.NET) resources
            if (disposing)
            {
                // we can use the IDisposable to explicitly clean up our temp file.
                if (File.Exists(_tempFile))
                    File.Delete(_tempFile);
            }

            // here we need to free unmanaged resources so this will be called by the finalizer below
        }

        // finalizer which is called by the garbage collector (GC)
        ~PdfHandler() { Dispose(false); } // if dispose was not call, the finalizer will release the unmanaged resources (false as parameter)

        // Dispose which is called by our code e.g. by using "using (var handler = new PdfHandler()){}"
        public void Dispose()
        {
            Dispose(true); // lets dispose all managed and unmanaged resources
            GC.SuppressFinalize(this); // if all managed resources are removed, we can call the GC to skip the finalizer
        }

        #endregion
    }
}