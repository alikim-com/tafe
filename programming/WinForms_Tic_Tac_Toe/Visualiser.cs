
namespace WinFormsApp1;

/// <summary>
/// Provides bridges between game and UI states
/// </summary>
internal class VBridge
{
    public static Dictionary<Game.Roster, CellWrapper.BgMode> toCellDef = new()
    {
        { Game.Roster.None, CellWrapper.BgMode.Default },
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
                throw new Exception($"VBridge.SyncBoardHandler : can't translate '{rec.Value}'");
            t.Add(rec.Key, bg);
        }

        EM.RaiseEvtSyncBoardUI(s ?? new { }, t);
    }

    internal static void PlayerConfirmedHandler(object? sender, string[] e)
    {
        if (!Enum.TryParse(e[0], out Game.Roster player))
            throw new Exception($"VBridge.PlayerConfirmedHandler : unknown player '{e[0]}'");
        if (!Enum.TryParse(e[1], out CellWrapper.BgMode cellBg))
            throw new Exception($"VBridge.PlayerConfirmedHandler : unknown cell bg '{e[1]}'");
        toCell.Add(player, cellBg);
    }
}




//   { Game.Roster.Human, CellWrapper.BgMode.PlayerLeft},
//   { Game.Roster.AI, CellWrapper.BgMode.PlayerRight},