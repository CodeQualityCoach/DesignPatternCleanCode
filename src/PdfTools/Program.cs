using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FSharp.Markdown;
using FSharp.Markdown.Pdf;
using iTextSharp.text;
using iTextSharp.text.pdf;
using QRCoder;
using Image = iTextSharp.text.Image;

namespace PdfTools
{
    public class Program
    {

        public static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                Console.WriteLine(arg);
            }

            if (args.Length == 0)
                throw new ArgumentException("at least an action is required");

            var action = args[0];

            // markdown-in, pdf-out
            if (string.Equals(action, "create", StringComparison.CurrentCultureIgnoreCase))
                DoCreate(args.Skip(1).ToArray());

            // pdf-in, qrcodetext, optional outfile
            if (string.Equals(action, "addcode", StringComparison.CurrentCultureIgnoreCase))
            {
                var enhancer = new PdfCodeEnhancer(args[1]);

                enhancer.AddTextAsCode(args[2]);

                if (args.Length == 4)
                    enhancer.SaveAs(args[3]);
                else
                    enhancer.SaveAs(args[1]);
            }

            // url, outfile
            if (string.Equals(action, "download", StringComparison.CurrentCultureIgnoreCase))
            {
                var client = new HttpClient();
                var response = client.GetAsync(args[1]).Result;
                var pdf = response.Content.ReadAsByteArrayAsync().Result;

                File.WriteAllBytes(args[2], pdf);
            }

            // url, outfile
            if (string.Equals(action, "archive", StringComparison.CurrentCultureIgnoreCase))
            {
                var archiver = new PdfArchiver();
                archiver.Archive(args[1]);
                archiver.SaveAs(args[2]);
            }

            // url, outfile
            if (string.Equals(action, "combine", StringComparison.CurrentCultureIgnoreCase))
            {
                CombineMultiplePDF(args.Skip(2).ToArray(), args[1]);
            }
        }

        private static void DoCreate(string[] args)
        {
            if (args.Length != 2)
                throw new ArgumentException("at least in and out parameter is required");

            var inFile = args[0];
            var outFile = args[1];

            var mdText = File.ReadAllText(inFile);
            var mdDoc = Markdown.Parse(mdText);

            MarkdownPdf.Write(mdDoc, outFile);
        }

        private static void CombineMultiplePDF(string[] fileNames, string outFile)
        {
            // step 1: creation of a document-object
            Document document = new Document();
            //create newFileStream object which will be disposed at the end
            using (FileStream newFileStream = new FileStream(outFile, FileMode.Create))
            {
                // step 2: we create a writer that listens to the document
                PdfCopy writer = new PdfCopy(document, newFileStream);

                // step 3: we open the document
                document.Open();

                foreach (string fileName in fileNames)
                {
                    // we create a reader for a certain document
                    PdfReader reader = new PdfReader(fileName);
                    reader.ConsolidateNamedDestinations();

                    // step 4: we add content
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                    }

                    //PRAcroForm form = reader.AcroForm;
                    //if (form != null)
                    //{
                    //    writer.AddDocument(reader);
                    //}

                    reader.Close();
                }

                // step 5: we close the document and writer
                writer.Close();
                document.Close();
            }//disposes the newFileStream object
        }
    }

    public class PdfArchiver
    {
        private readonly string _tempFile;
        private readonly IBarcodeGenerator _barcodeGenerator;
        private readonly IHttpClient _httpClient;

        public PdfArchiver(IBarcodeGenerator barcodeGenerator = null,
            IHttpClient httpClient = null)
        {
            _tempFile = Path.GetTempFileName();
            // here we use the static service locator
            _barcodeGenerator = barcodeGenerator ?? throw new ArgumentNullException(nameof(barcodeGenerator));
            _httpClient = httpClient ?? new HttpClientWrapper();
        }

        public void Archive(string url)
        {
            var response = _httpClient.GetAsync(url).Result;
            var pdf = response.Content.ReadAsByteArrayAsync().Result;

            var tmpTempFile = Path.GetTempFileName();
            File.WriteAllBytes(tmpTempFile, pdf);

            using (Stream inputPdfStream = new FileStream(tmpTempFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream inputImageStream = new MemoryStream())
            using (Stream outputPdfStream = new FileStream(_tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var code = _barcodeGenerator.CreateTextBarcode(url);
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

    public class HttpClientWrapper : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public HttpClientWrapper()
        {
            _httpClient = new HttpClient();
        }
        public Task<HttpResponseMessage> GetAsync(string url)
        {
            return _httpClient.GetAsync(url);
        }
    }

    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }

    public class PdfCodeEnhancer
    {
        private readonly string _pdfFile;
        private readonly IMyLogger _logger;
        private readonly string _tempFile;
        private readonly IBarcodeGenerator _barcodeGenerator;

        public PdfCodeEnhancer(string pdfFile, IBarcodeGenerator barcodeGenerator = null, IMyLogger logger = null)
        {
            _pdfFile = pdfFile;
            _logger = logger ?? new EmptyMyLogger();
            _tempFile = Path.GetTempFileName();
            // here we use the static service locator
            _barcodeGenerator = barcodeGenerator ?? throw new ArgumentNullException(nameof(barcodeGenerator));
        }

        public void AddTextAsCode(string text)
        {
            _logger.Log($"Enhancing PDF with Text: {text}");

            using (Stream inputPdfStream = new FileStream(_pdfFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream inputImageStream = new MemoryStream())
            using (Stream outputPdfStream = new FileStream(_tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var code = _barcodeGenerator.CreateTextBarcode(text);
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

    public interface IMyLogger
    {
        void Log(string message);
    }

    public class EmptyMyLogger : IMyLogger
    {
        public void Log(string message) { }
    }

    public class MyConsoleLogger : IMyLogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class MyTraceLogger : IMyLogger
    {
        public void Log(string message)
        {
            Trace.WriteLine(message);
        }
    }

    #region refactored code

    public interface IBarcodeGenerator
    {
        Bitmap CreateTextBarcode(string text);
    }

    /// <summary>
    /// Create a QR barcode using QRCoder
    /// </summary>
    public class CQCoderBarcodeGeneratorSmall : IBarcodeGenerator
    {
        public Bitmap CreateTextBarcode(string text)
        {
            var qrCodeGenerator = new QRCodeGenerator();
            var qrCodeData = qrCodeGenerator.CreateQrCode(new PayloadGenerator.Url(text), QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);

            return qrCode.GetGraphic(1);
        }
    }

    /// <summary>
    /// Create a QR barcode using QRCoder
    /// </summary>
    public class QRCoderBarcodeGenerator : IBarcodeGenerator
    {
        public Bitmap CreateTextBarcode(string text)
        {
            var qrCodeGenerator = new QRCodeGenerator();
            var qrCodeData = qrCodeGenerator.CreateQrCode(new PayloadGenerator.Url(text), QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);

            return qrCode.GetGraphic(3);
        }
    }

    public static class ServiceLocator
    {
        // this just creates it "thread safe" with a static initializer
        private static IBarcodeGenerator _barcodeGenerator = new QRCoderBarcodeGenerator();

        // But I would like to have a singleton
        //private static  _barcodeGenerator;

        public static IBarcodeGenerator BarcodeGenerator
        {
            get { return _barcodeGenerator; }
        }
    }

    #endregion
}
