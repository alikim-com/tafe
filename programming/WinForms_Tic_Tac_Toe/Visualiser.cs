
namespace WinFormsApp1;

/// <summary>
/// Provides bridges between game and UI states
/// </summary>
internal class VBridge
{
    static readonly Dictionary<Game.Roster, CellWrapper.BgMode> toCellDef = new()
    {
        { Game.Roster.None, CellWrapper.BgMode.Default },
    };

    static Dictionary<Game.Roster, CellWrapper.BgMode> toCell = new(toCellDef);

    /// <summary>
    /// Adds all players choices from clicking on cfg panels to the dictionary
    /// </summary>
    static public void AddCfg(Dictionary<Game.Roster, CellWrapper.BgMode> toCellCfg)
    {
        foreach(var rec in toCellCfg) toCell.Add(rec.Key, rec.Value);
    }

    /// <summary>
    /// Subscribed to EM.EvtReset event
    /// </summary>
    static public void ResetHandler(object? s, EventArgs e) => toCell = new(toCellDef);

    /// <summary>
    /// Subscribed to EM.EvtSyncBoard event<br/>
    /// Translates game board state into UI states
    /// </summary>
    static public void SyncBoardHandler(object? s, Dictionary<Point, Game.Roster> e)
    {
        Dictionary<Point, CellWrapper.BgMode> t = new();
        foreach (var rec in e) {
            if (!toCell.TryGetValue(rec.Value, out CellWrapper.BgMode bg))
                throw new Exception($"VBridge.SyncBoardHandler : can't translate '{rec.Value}'");
            t.Add(rec.Key, bg);
        }

        EM.RaiseEvtSyncBoardUI(s ?? new { }, t);
    }    
}
