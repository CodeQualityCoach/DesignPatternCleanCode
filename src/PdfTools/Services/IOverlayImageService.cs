using System;
using System.Drawing;

namespace PdfTools.Services
{
    public interface IOverlayImageService
    {
        Bitmap CreateOverlayImage(Uri uri);
        Bitmap CreateOverlayImage(string text);
    }
}