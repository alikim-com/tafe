
namespace WinFormsApp1;

/// <summary>
/// Manages events
/// </summary>
internal class EM
{
    /// <summary>
    /// Translates game states into UI states in VBridge
    /// </summary>
    static public event EventHandler<Dictionary<Point, Game.Roster>> EvtSyncBoard = delegate { };
    /// <summary>
    /// Sync the board with EvtSyncBoard translation done by VBridge
    /// </summary>
    static public event EventHandler<Dictionary<Point, CellWrapper.BgMode>> EvtSyncBoardUI = delegate { };
    /// <summary>
    /// Resets UI, except the board
    /// </summary>
    static public event EventHandler EvtReset = delegate { };
    /// <summary>
    /// Raised when a player clicks on a cell
    /// </summary>
    static public event EventHandler<Point> EvtPlayerMoved = delegate { };
    /// <summary>
    /// Confirms a player visual appearance
    /// </summary>
    static public event EventHandler<CellWrapper.BgMode>EvtPlayerConfigured = delegate { };

    // ----- wrappers -----

    /// <summary>
    /// EvtPlayerConfigured wrapper, multi-thread safe
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public static void RaiseEvtPlayerConfigured(object sender, CellWrapper.BgMode e)
    {
        var handler = EvtPlayerConfigured;
        handler?.Invoke(sender, e);
    }
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
    /// <summary>
    /// EvtSyncBoard wrapper, multi-thread safe
    /// </summary>
    /// <param name="e">List of cells to update,<br/>
    /// containing row(X) and column(Y) of a cell and the player occupying it</param>
    public static void RaiseEvtSyncBoard(object sender, Dictionary<Point, Game.Roster> e)
    {
        var handler = EvtSyncBoard;
        handler?.Invoke(sender, e);
    }
    /// <summary>
    /// EvtSyncBoardUI wrapper, multi-thread safe
    /// </summary>
    /// <param name="e">List of cells to update,<br/>
    /// containing row(X) and column(Y) of a cell and a background associated with the player</param>
    public static void RaiseEvtSyncBoardUI(object sender, Dictionary<Point, CellWrapper.BgMode> e)
    {
        var handler = EvtSyncBoardUI;
        handler?.Invoke(sender, e);
    }
}
