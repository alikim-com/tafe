﻿
namespace WinFormsApp1;

/// <summary>
/// Manages events
/// </summary>
internal class EM
{
    /// <summary>
    /// Translates game states into UI states in VBridge
    /// </summary>
    /// <param>List of cells to update,<br/>
    /// containing row(X) and column(Y) of a cell and the player occupying it</param>
    static event EventHandler<Dictionary<Point, Game.Roster>> EvtSyncBoard = delegate { };
    /// <summary>
    /// Sync the board with EvtSyncBoard translation done by VBridge
    /// </summary>
    /// <param>List of cells to update,<br/>
    /// containing row(X) and column(Y) of a cell and a background associated with the player</param>
    static event EventHandler<Dictionary<Point, CellWrapper.BgMode>> EvtSyncBoardUI = delegate { };
    /// <summary>
    /// Resets UI, except the board
    /// </summary>
    static event EventHandler EvtReset = delegate { };
    /// <summary>
    /// Raised when a player clicks on a cell
    /// </summary>
    /// <param>Point containing row(X) and column(Y) of the cell clicked</param>
    static event EventHandler<Point> EvtPlayerMoved = delegate { };
    /// <summary>
    /// Confirms a player visual appearance (cell bg) on PanelWrapper click
    /// </summary>
    static event EventHandler<CellWrapper.BgMode> EvtPlayerConfigured = delegate { };
    /// <summary>
    /// AI choice of a config panel for TurnWheel to simulate click on it
    /// </summary>
    static event EventHandler<int> EvtAIMoved = delegate { };
    /// <summary>
    /// Issued by TurnWheel when the game is ready to be played
    /// </summary>
    static event EventHandler EvtConfigFinished = delegate { };
    /// <summary>
    /// Updates labels in LabelManager
    /// </summary>
    static event EventHandler<Enum[]> EvtUpdateLabels = delegate { };

    /// <summary>
    /// Event associations
    /// </summary>
    public enum Evt
    {
        SyncBoard,
        SyncBoardUI,
        Reset,
        PlayerMoved,
        PlayerConfigured,
        AIMoved,
        ConfigFinished,
        UpdateLabels
    }
    /// <summary>
    /// To raise or sub/unsub to events by their enum names
    /// </summary>
    static readonly Dictionary<Evt, Delegate> dict = new() {
        { Evt.SyncBoard, EvtSyncBoard },
        { Evt.SyncBoardUI, EvtSyncBoardUI },
        { Evt.Reset, EvtReset },
        { Evt.PlayerMoved, EvtPlayerMoved },
        { Evt.PlayerConfigured, EvtPlayerConfigured },
        { Evt.AIMoved, EvtAIMoved },
        { Evt.ConfigFinished, EvtConfigFinished },
        { Evt.UpdateLabels, EvtUpdateLabels },
    };

    // ----- event wrappers -----

    /// <summary>
    /// Multi-thread safe wrapper for raising events
    /// </summary>
    /// <param name="evt">Event to be raised</param>
    /// <param name="sender">Event sender object</param>
    /// <param name="e">Event arguments</param>
    static public void Raise<E>(Evt enm, object sender, E e)
    {
        if (!dict.TryGetValue(enm, out var evt))
            throw new NotImplementedException($"EM.Raise : no event for Evt.{enm}");

        bool generic = dict[enm].GetType() == typeof(EventHandler);

        if (generic)
            ((EventHandler)dict[enm])?.Invoke(sender, new EventArgs());

        else
            ((EventHandler<E>)dict[enm])?.Invoke(sender, e);
    }

    static public void Subscribe(Evt enm, Delegate handler)
    {
        if (!dict.TryGetValue(enm, out var evt))
            throw new NotImplementedException($"EM.Subscribe : no event for Evt.{enm}");

            dict[enm] = Delegate.Combine(evt, handler);
    }

    static public void Unsubscribe(Evt enm, Delegate handler)
    {
        if (!dict.TryGetValue(enm, out var evt))
            throw new NotImplementedException($"EM.Unsubscribe : no event for Evt.{enm}");

        dict[enm] = Delegate.Remove(evt, handler) ?? (() => { });
    }

    // ----- cross-thread calls -----

    public static AppForm? uiThread;

    /// <summary>
    /// Raise events from UI thread for safe UI access
    /// </summary>
    /// <param name="lambda"></param>
    public static void InvokeFromMainThread(Action lambda) => uiThread?.Invoke(lambda);
}