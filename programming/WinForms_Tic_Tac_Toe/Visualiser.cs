
namespace WinFormsApp1;

/// <summary>
/// Provides bridges between game and UI states
/// </summary>
internal class VBridge
{
    static Dictionary<ChoiceItem.Side, CellWrapper.BgMode> sideBg = new()
    {
        { ChoiceItem.Side.Left, CellWrapper.BgMode.Player1 },
        { ChoiceItem.Side.Right, CellWrapper.BgMode.Player2 }
    };

    static readonly Dictionary<Game.Roster, CellWrapper.BgMode> toCellDef = new()
    {
        { Game.Roster.None, CellWrapper.BgMode.Default },
    };

    static Dictionary<Game.Roster, CellWrapper.BgMode> toCell = new();

    /// <summary>
    /// Create translation table from game states to cell backgrounds
    /// </summary>
    static public void Reset(IEnumerable<ChoiceItem> chosen)
    {
        toCell = new(toCellDef);

        foreach (var chItem in chosen)
        {
            var side = chItem.side;
            if (!sideBg.TryGetValue(side, out CellWrapper.BgMode bg))
                throw new Exception($"VBridge.Reset : can't translate '{side}'");

            toCell.Add(chItem.rosterId, bg);
        }
    }

    /// <summary>
    /// Subscribed to EM.EvtSyncBoard event<br/>
    /// Translates game board state into UI states
    /// </summary>
    static public EventHandler<Dictionary<Point, Game.Roster>> SyncBoardHandler = (object? s, Dictionary<Point, Game.Roster> e) =>
    {
        Dictionary<Point, CellWrapper.BgMode> t = new();
        foreach (var rec in e)
        {
            if (!toCell.TryGetValue(rec.Value, out CellWrapper.BgMode bg))
                throw new Exception($"VBridge.SyncBoardHandler : can't translate '{rec.Value}'");
            t.Add(rec.Key, bg);
        }

        EM.Raise(EM.Evt.SyncBoardUI, s ?? new { }, t);
    };
}
