namespace imageUtility;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

internal class ImageUtility
{
    public static Image GetOverlayOnBackground(Size dstSize, Image src, Size offsetTL)
    {
        Bitmap dst = new(dstSize.Width, dstSize.Height);
        using (Graphics g = Graphics.FromImage(dst))
        {
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(src, offsetTL.Width, offsetTL.Height, dst.Width, dst.Height);
            g.Save();
        }
        return dst; // size vs point
    }

    public static Image GetImageCopyWithAlpha(Image src, float opacity)
    {
        Bitmap dst = new(src.Width, src.Height);
        using (Graphics g = Graphics.FromImage(dst))
        {
            ColorMatrix matrix = new()
            {
                Matrix33 = opacity
            };
            ImageAttributes attributes = new();
            attributes.SetColorMatrix(
                matrix,
                ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap
            );
            g.DrawImage(
                src,
                new Rectangle(0, 0, dst.Width, dst.Height),
                0, 0, src.Width, src.Height,
                GraphicsUnit.Pixel,
                attributes
            );
        }
        return dst;
    }
}
