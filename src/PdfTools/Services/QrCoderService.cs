using System;
using System.Drawing;
using QRCoder;

namespace PdfTools.Services
{
    internal class QrCoderService : IOverlayImageService
    {
        private const int GraphicSize = 2;
        private const QRCodeGenerator.ECCLevel Level = QRCodeGenerator.ECCLevel.Q;

        public Bitmap CreateOverlayImage(Uri uri)
        {

            var qrCodeGenerator = new QRCodeGenerator();
            var payload = new PayloadGenerator.Url(uri.AbsoluteUri);
            var qrCodeData = qrCodeGenerator.CreateQrCode(payload, Level);
            var qrCode = new QRCode(qrCodeData);

            return qrCode.GetGraphic(GraphicSize);
        }

        public Bitmap CreateOverlayImage(string text)
        {
            var qrCodeGenerator = new QRCodeGenerator();
            var qrCodeData = qrCodeGenerator.CreateQrCode(text, Level);
            var qrCode = new QRCode(qrCodeData);

            return qrCode.GetGraphic(GraphicSize);
        }
    }
}
