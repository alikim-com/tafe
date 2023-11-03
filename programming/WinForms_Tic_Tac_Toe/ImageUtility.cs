namespace imageUtility;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

internal class ImageUtility
{
    public static Color BlendColors(Color clr1, Color clr2)
    {
        byte alpha = (byte)(clr1.A + clr2.A - clr1.A * clr2.A / 255);

        double nmr = 1 / (255 * alpha);
        byte newR = (byte)((clr1.R * clr1.A + clr2.R * clr2.A - clr1.R * clr2.A) * nmr);
        byte newG = (byte)((clr1.G * clr1.A + clr2.G * clr2.A - clr1.G * clr2.A) * nmr);
        byte newB = (byte)((clr1.B * clr1.A + clr2.B * clr2.A - clr1.B * clr2.A) * nmr);

        return Color.FromArgb(alpha, newR, newG, newB);
    }

    /// <summary>
    /// Fit small rectangle into a large one,<br/>
    /// while preserving small box's aspect ratio and maximising its area
    /// </summary>
    /// <param name="large">Size of a larger rectangle</param>
    /// <param name="small">Size of a smaller rectangle</param>
    /// <returns>The new size of a scaled small box</returns>
    public static Size FitRect(Size large, Size small)
    {
        double widthRatio = (double)large.Width / small.Width;
        double heightRatio = (double)large.Height / small.Height;

        double scale = Math.Min(widthRatio, heightRatio);

        return new Size(
            (int)(small.Width * scale), 
            (int)(small.Height * scale)
        );
    }

    public static Image GetOverlayOnBackground(Size dstSize, Image src, string align)
    {
        Size scaledSrc = FitRect(dstSize, src.Size);
        Size offsetTL = align switch
        {
            "left" => new(0, 0),
            "right" => new(dstSize.Width - scaledSrc.Width, 0),
            _ => throw new NotImplementedException($"GetOverlayOnBackground : align '{align}'"),
        };

        Bitmap dst = new(dstSize.Width, dstSize.Height);
        using (Graphics g = Graphics.FromImage(dst))
        {
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(src, offsetTL.Width, offsetTL.Height, scaledSrc.Width, scaledSrc.Height);
            g.Save();
        }
        return dst;
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
