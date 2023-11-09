using System.Collections.Generic;

namespace WinFormsApp1;

/// <summary>
/// Provides bridges between game and UI states
/// </summary>
internal class VBridge
{
    public static Dictionary<Game.Roster, CellWrapper.BgMode> toCellDef = new()
    {
        { Game.Roster.None, CellWrapper.BgMode.Default },
     { Game.Roster.Human, CellWrapper.BgMode.PlayerLeft},
     { Game.Roster.AI, CellWrapper.BgMode.PlayerRight},
    };

    public static Dictionary<Game.Roster, CellWrapper.BgMode> toCell = new(toCellDef);

    /// <summary>
    /// Subscribed to EM.EvtReset event
    /// </summary>
    public static void ResetHandler(object? s, EventArgs e) => toCell = new(toCellDef);

    /// <summary>
    /// Subscribed to EM.EvtSyncBoard event<br/>
    /// Translates game board state into UI states
    /// </summary>
    public static void SyncBoardHandler(object? s, Dictionary<Point, Game.Roster> e)
    {
        Dictionary<Point, CellWrapper.BgMode> t = new();
        foreach (var rec in e) {
            if (!toCell.TryGetValue(rec.Value, out CellWrapper.BgMode bg))
                throw new Exception("VBridge.SyncBoardHandler : can't translate '{rec.Value}'");
            t.Add(rec.Key, bg);
        }

        EM.RaiseEvtSyncBoardUI(s, t);
    }
}




//   { Game.Roster.Human, CellWrapper.BgMode.PlayerLeft},
//   { Game.Roster.AI, CellWrapper.BgMode.PlayerRight},