using System;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using iTextSharp.text.pdf;
using PdfTools.Services;
using Image = iTextSharp.text.Image;

namespace PdfTools.Handler
{
    public class PdfHandler
    {
        private readonly IOverlayImageService _imageGenerator;
        private readonly string _tempFile;

        public PdfHandler(IOverlayImageService imageGenerator = null)
        {
            _imageGenerator = imageGenerator ?? new QrCoderService();
            _tempFile = Path.GetTempFileName();
        }

        public void Open(string filepath)
        {
            File.Copy(filepath, _tempFile, true);
        }

        public void Download(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
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
    }
}