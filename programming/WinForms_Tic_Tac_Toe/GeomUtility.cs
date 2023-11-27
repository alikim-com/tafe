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

/// <summary>
/// For keeping fractional position of a control inside its parent
/// </summary>
public class RatioPosition
{
    static readonly Dictionary<Control, Control> fam = new();
    static readonly Dictionary<Control, RectangleF> anchors = new();

    static public void Add(Control ctrl, Control parent)
    {
        fam.Add(ctrl, parent);
        var clSize = parent.ClientSize;
        anchors.Add(ctrl, new RectangleF(
            (float)ctrl.Location.X / clSize.Width,
            (float)ctrl.Location.Y / clSize.Height,
            (float)ctrl.ClientSize.Width / clSize.Width,
            (float)ctrl.ClientSize.Height / clSize.Height
        ));
    }

    static public void Remove(Control ctrl)
    {
        fam.Remove(ctrl); 
        anchors.Remove(ctrl);
    }

    static public void Update(Control ctrl)
    {
        if (!fam.TryGetValue(ctrl, out Control? parent) || !anchors.TryGetValue(ctrl, out RectangleF rect)) return;

        ctrl.Location = new Point(
            (int)(rect.Left * parent.ClientSize.Width),
            (int)(rect.Top * parent.ClientSize.Height)
        );
    }
}