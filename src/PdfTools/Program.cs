using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
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

        public PdfArchiver()
        {
            _tempFile = Path.GetTempFileName();
        }
        public void Archive(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var pdf = response.Content.ReadAsByteArrayAsync().Result;

            var tmpTempFile = Path.GetTempFileName();
            File.WriteAllBytes(tmpTempFile, pdf);

            using (Stream inputPdfStream = new FileStream(tmpTempFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream inputImageStream = new MemoryStream())
            using (Stream outputPdfStream = new FileStream(_tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var code = CreateInitCode(url);
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

        private Bitmap CreateInitCode(string text)
        {
            var qrCodeGenerator = new QRCodeGenerator();
            var qrCodeData = qrCodeGenerator.CreateQrCode(new PayloadGenerator.Url(text), QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);

            return qrCode.GetGraphic(2);
        }

        public void SaveAs(string destFile)
        {
            File.Copy(_tempFile, destFile, true);
        }
    }

    public class PdfCodeEnhancer
    {
        private readonly string _pdfFile;
        private readonly string _tempFile;

        public PdfCodeEnhancer(string pdfFile)
        {
            _pdfFile = pdfFile;
            _tempFile = Path.GetTempFileName();
        }

        public void AddTextAsCode(string text)
        {
            using (Stream inputPdfStream = new FileStream(_pdfFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream inputImageStream = new MemoryStream())
            using (Stream outputPdfStream = new FileStream(_tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var code = CreateInitCode(text);
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

        private Bitmap CreateInitCode(string text)
        {
            var qrCodeGenerator = new QRCodeGenerator();
            var qrCodeData = qrCodeGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);

            return qrCode.GetGraphic(2);
        }

        public void SaveAs(string destFile)
        {
            File.Copy(_tempFile, destFile, true);
        }
    }
}
