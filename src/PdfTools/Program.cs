using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using iTextSharp.text.pdf;
using PdfTools.Commands;
using PdfTools.Services;
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
        private readonly IOverlayImageService _imageGenerator;
        private readonly string _tempFile;

        public PdfArchiver(IOverlayImageService imageGenerator = null)
        {
            _imageGenerator = imageGenerator ?? new QrCoderService();
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
                var code = _imageGenerator.CreateOverlayImage(new Uri(url));
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

    public class PdfCodeEnhancer
    {
        private readonly string _pdfFile;
        private readonly IOverlayImageService _imageGenerator;
        private readonly string _tempFile;

        public PdfCodeEnhancer(string pdfFile, IOverlayImageService imageGenerator = null)
        {
            _pdfFile = pdfFile;
            _imageGenerator = imageGenerator ?? new QrCoderService();
            _tempFile = Path.GetTempFileName();
        }

        public void AddTextAsCode(string text)
        {
            using (Stream inputPdfStream = new FileStream(_pdfFile, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream inputImageStream = new MemoryStream())
            using (Stream outputPdfStream = new FileStream(_tempFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                var code = _imageGenerator.CreateOverlayImage(text);
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
