using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using iTextSharp.text.pdf;
using PdfTools.Commands;
using QRCoder;
using Image = iTextSharp.text.Image;
// ReSharper disable StringLiteralTypo

namespace PdfTools
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Beware: there is one significant change: The commands only know the subset of args[] without the command name!
            var commandName = (args.Length > 0) ? args[0] : string.Empty;
            var commandContext = args.Skip(1).ToArray(); // we need to make this ToArray(), otherwise it is an IEnumerable

            // We simply create a dictionary, with the command name as key. Thanks to reflection and attributes.
            var availableCommands = CommandHelper.GetCommands().ToDictionary(x => x.GetName(), x => x);

            // now, we get the command instance
            if (!availableCommands.TryGetValue(commandName, out var commandInstance))
            {
                Console.WriteLine($"Cannot find command: '{commandName}'\r\n");
                commandInstance = new HelpCommand();
            }

            // and check if the command can be executed
            if (commandInstance.CanExecute(commandContext))
                commandInstance.Execute(commandContext);
            else
                Console.WriteLine(commandInstance.Usage);
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
