namespace geomUtility;

public class GeomUtility
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

}
