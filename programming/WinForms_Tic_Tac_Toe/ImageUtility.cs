namespace imageUtility;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

public static class ColorExtensions
{
    public struct _argb
    {
        public double a;
        public double r;
        public double g;
        public double b;

        public override readonly string ToString() => $"a: {a}, r: {r}, g: {g}, b: {b}";
    }
    public static _argb Normalise(this Color c)
    {
        double k = 1.0 / 255;
        return new _argb { a = c.A * k, r = c.R * k, g = c.G * k, b = c.B * k };
    }

    public static Color FromNormalised(_argb nrm) => Color.FromArgb(
        (byte)(nrm.a * 255),
        (byte)(nrm.r * 255),
        (byte)(nrm.g * 255),
        (byte)(nrm.b * 255)
    );

    /// <summary>
    /// Blends top ARGB pixel over bottom ARGB pixel
    /// --- no pre ---
    /// a = a1 + a2 - a1 * a2
    /// aR = R2 * a2 + R1 * a1 * (1 - a2)
    /// R = aR / a
    /// ---  pre  ---
    /// a = a2 + a1 * (1 - a2)
    /// r = r2 + r1 * (1 - a2)
    /// </summary>
    /// <param name="top">Transparent pixel on top</param>
    /// <param name="bot">Transparent pixel below</param>
    /// <param name="pre">Whether RGBs are premultiplied by alpha</param>
    /// <returns></returns>
    public static Color BlendOver(Color top, Color bot, bool pre = false)
    {
        var t = top.Normalise();
        var b = bot.Normalise();
        double f = 1 - t.a;
        
        double a = t.a + b.a * f;
        if (a == 0) return Color.FromArgb(0, 0, 0, 0);

        double ar = 1 / a;
        double _r = pre ? t.r + b.r * f : (t.r * t.a + b.r * b.a * f) * ar;
        double _g = pre ? t.g + b.g * f : (t.g * t.a + b.g * b.a * f) * ar;
        double _b = pre ? t.b + b.b * f : (t.b * t.a + b.b * b.a * f) * ar;

        return FromNormalised(new _argb() { a = a, r = _r, g = _g, b = _b });
    }
}

public class ImageUtility
{
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

    public static Image GetOverlayOnBackground(Size dstSize, Image src, Color? bg, string align)
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
            g.Clear(bg ?? Color.Transparent);
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
