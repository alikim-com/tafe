namespace WinFormsApp1;

/// <summary>
/// Manages events
/// </summary>
internal class EM
{
    /// <summary>
    /// Resets the game
    /// </summary>
    static public event EventHandler EvtReset = delegate { };
    /// <summary>
    /// Raised when a player clicks on a cell
    /// </summary>
    static public event EventHandler<Point> EvtPlayerMoved = delegate { };
    /// <summary>
    /// EvtReset wrapper, multi-thread safe
    /// </summary>
    public static void RaiseEvtReset(object sender, EventArgs e)
    {
        var handler = EvtReset;
        handler?.Invoke(sender, e);
    }
    /// <summary>
    /// EvtPlayerMoved wrapper, multi-thread safe
    /// </summary>
    /// <param name="e">Point containing row(X) and column(Y) of the cell clicked</param>
    public static void RaiseEvtPlayerMoved(object sender, Point e)
    {
        var handler = EvtPlayerMoved;
        handler?.Invoke(sender, e);
    }
}
