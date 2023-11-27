
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

public class RatioPosControl
{
    public enum Anchor
    {
        Left, Top, Right, Bottom
    }

    public readonly Anchor hor, ver;

    public readonly Control control;
    public readonly Control parent;

    public readonly RectangleF ratioAnchors;

    public RatioPosControl(Control _control, Control _parent, Anchor _hor, Anchor _ver)
    {
        control = _control;
        parent = _parent;
        hor = _hor;
        ver = _ver;

        var clSize = parent.ClientSize;
        ratioAnchors = new RectangleF(
            (float)control.Location.X / clSize.Width,
            (float)control.Location.Y / clSize.Height,
            (float)control.ClientSize.Width / clSize.Width,
            (float)control.ClientSize.Height / clSize.Height
        );
    }
}

/// <summary>
/// For keeping fractional position of a control inside its parent
/// </summary>
public class RatioPosition
{
    static public readonly List<RatioPosControl> rpControls = new();

    static public void Add(Control ctrl, Control parent, RatioPosControl.Anchor hor, RatioPosControl.Anchor ver) =>
        rpControls.Add(new RatioPosControl(ctrl, parent, hor, ver));       

    static public void Remove(Control control) => rpControls.RemoveAll(rpCtrl => rpCtrl.control == control);

    static public void Update(Control control)
    {
        var rpCtrl = rpControls.Find(rpCtrl => rpCtrl.control == control);
        if (rpCtrl == null) return;

        var ctrl = rpCtrl.control;
        var rect = rpCtrl.ratioAnchors;
        var clSize = rpCtrl.parent.ClientSize;
        var hor = rpCtrl.hor; 
        var ver = rpCtrl.ver;

        int x;
        if (hor == RatioPosControl.Anchor.Left)
            x = (int)(rect.Left * clSize.Width);
        else if (hor == RatioPosControl.Anchor.Right)
            x = (int)(rect.Right * clSize.Width) - ctrl.Width;
        else
            throw new Exception($"RatioPosition.Update : wrong hor Anchor type '{hor}'");

        int y;
        if (ver == RatioPosControl.Anchor.Top)
            y = (int)(rect.Top * clSize.Height);
        else if (ver == RatioPosControl.Anchor.Bottom)
            y = (int)(rect.Bottom * clSize.Height) - ctrl.Height;
        else
            throw new Exception($"RatioPosition.Update : wrong ver Anchor type '{ver}'");

        rpCtrl.control.Location = new Point(x, y);
    }
}
