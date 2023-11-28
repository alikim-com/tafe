
namespace WinFormsApp1;

/// <summary>
/// Provides bridges between game and UI states
/// </summary>
internal class VBridge
{
    // CellWrapper

    static readonly Dictionary<ChoiceItem.Side, CellWrapper.BgMode> sideToCellBg = new()
    {
        { ChoiceItem.Side.Left, CellWrapper.BgMode.Player1 },
        { ChoiceItem.Side.Right, CellWrapper.BgMode.Player2 }
    };

    static readonly Dictionary<Game.Roster, CellWrapper.BgMode> rosterToCellBg = new()
    {
        { Game.Roster.None, CellWrapper.BgMode.Default }
    };

    // LabelManager

    static readonly Dictionary<ChoiceItem.Side, LabelManager.Info> sideToLabMgr = new()
    {
        { ChoiceItem.Side.Left, LabelManager.Info.Player1 },
        { ChoiceItem.Side.Right, LabelManager.Info.Player2 }
    };

    static readonly Dictionary<Game.Roster, LabelManager.Info> rosterToLabMgr = new();

    /// <summary>
    /// Create translation table from game states to cell backgrounds
    /// </summary>
    static public void Reset(IEnumerable<ChoiceItem> chosen)
    {
        while (rosterToCellBg.Count > 1) rosterToCellBg.Remove(rosterToCellBg.Keys.Last());

        rosterToLabMgr.Clear();

        Dictionary<LabelManager.Info, string> playerInfo = new();

        foreach (var chItem in chosen)
        {
            var side = chItem.side;
            if (!sideToCellBg.TryGetValue(side, out CellWrapper.BgMode bg))
                throw new Exception($"VBridge.Reset : can't translate '{side}'");

            rosterToCellBg.Add(chItem.rosterId, bg);

            if (!sideToLabMgr.TryGetValue(side, out LabelManager.Info state))
                throw new Exception($"VBridge.Reset : can't translate '{side}'");

            playerInfo.Add(state, chItem.identityName);

            var msg = chItem.originType == "AI" ? 
                $"{chItem.identityName} is moving..." : $"Your move, {chItem.identityName}...";

            if(!Enum.TryParse($"{state}Move", out LabelManager.Info stateMove))
                throw new Exception($"VBridge.Reset : couldn't find '{state}Move'");

            playerInfo.Add(stateMove, msg);
        }

        LabelManager.Reset(playerInfo);

        // set players names on the bottom labels
        EM.Raise(EM.Evt.UpdateLabels, new { }, new Enum[] { 
            LabelManager.Info.Player1, 
            LabelManager.Info.Player2,
        });
    }

    /*
     // from turnWheel
     stsic public EvenhHandler UpdateLabel () => {
            EM.Raise(EM.Evt.UpdateLabelUI, s ?? new { }, t); -> lanMgr
        }
     */


    /// <summary>
    /// Subscribed to EM.EvtSyncBoard event<br/>
    /// Translates game board state into UI states
    /// </summary>
    static public EventHandler<Dictionary<Point, Game.Roster>> SyncBoardHandler = (object? s, Dictionary<Point, Game.Roster> e) =>
    {
        Dictionary<Point, CellWrapper.BgMode> cellBgs = new();
        foreach (var (rowCol, rostId) in e)
        {
            if (!rosterToCellBg.TryGetValue(rostId, out CellWrapper.BgMode bg))
                throw new Exception($"VBridge.SyncBoardHandler : can't translate '{rostId}'");
            cellBgs.Add(rowCol, bg);
        }

        EM.Raise(EM.Evt.SyncBoardUI, s ?? new { }, cellBgs);
    };
}
