using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text.RegularExpressions;

namespace Trail.Application.Services.Pdf
{
    /// \(summary\)
    /// Utilities for handling images in pdfs
    /// generated them.
    /// \(/summary\)
    public static class ImageHandling
    {
        public static Bitmap FitImageToPage(Image image, int pageWidth, int pageHeight)
        {

            double greaterAspect = 1;

            if (pageHeight < image.Size.Height || pageWidth < image.Size.Width)
            {
                var widthAspect = image.Size.Width / (double)pageWidth;
                var heightAspect = image.Size.Height / (double)pageHeight;

                greaterAspect = Math.Max(widthAspect, heightAspect);
            }

            var newWidth = (int)(image.Size.Width / greaterAspect);
            var newHeight = (int)(image.Size.Height / greaterAspect);


            var destRect = new Rectangle(0, 0, newWidth, newHeight);
            var destImage = new Bitmap(newWidth, newHeight);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using var wrapMode = new ImageAttributes();
                wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
            }

            return destImage;
        }

    }
}
