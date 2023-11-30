
namespace WinFormsApp1;

/// <summary>
/// Provides bridges between game and UI states
/// </summary>
internal class VBridge
{
    // CellWrapper

    /// <summary>
    /// Auxiliary map
    /// </summary>
    static readonly Dictionary<ChoiceItem.Side, CellWrapper.BgMode> sideToCellBg = new()
    {
        { ChoiceItem.Side.Left, CellWrapper.BgMode.Player1 },
        { ChoiceItem.Side.Right, CellWrapper.BgMode.Player2 }
    };

    /// <summary>
    /// used in SyncBoardHandler
    /// </summary>
    static readonly Dictionary<Game.Roster, CellWrapper.BgMode> rosterToCellBg = new()
    {
        { Game.Roster.None, CellWrapper.BgMode.Default }
    };

    // LabelManager

    /// <summary>
    /// Auxiliary map
    /// </summary>
    static readonly Dictionary<ChoiceItem.Side, LabelManager.Info> sideToLabMgr = new()
    {
        { ChoiceItem.Side.Left, LabelManager.Info.Player1 },
        { ChoiceItem.Side.Right, LabelManager.Info.Player2 }
    };

    /// <summary>
    /// used in SyncMoveLabelsHandler
    /// </summary>
    static readonly Dictionary<Game.Roster, LabelManager.Info> rosterToLabMgr = new();

    /// <summary>
    /// Create translation table from game states to cell backgrounds
    /// </summary>
    static internal void Reset(IEnumerable<ChoiceItem> chosen)
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

            rosterToLabMgr.Add(chItem.rosterId, stateMove);
        }

        LabelManager.Reset(playerInfo);

        EM.Raise(EM.Evt.UpdateLabels, new { }, new Enum[] { 
            LabelManager.Info.Player1, 
            LabelManager.Info.Player2,
        });
    }

    /// <summary>
    /// Subscribed to EM.EvtSyncBoard event<br/>
    /// Translates game board state into UI states
    /// </summary>
    static internal EventHandler<Dictionary<Point, Game.Roster>> SyncBoardHandler = (object? s, Dictionary<Point, Game.Roster> e) =>
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

    /// <summary>
    /// Subscribed to EM.SyncMoveLabels event raised by TurnWheel to update labels on player move
    /// </summary>
    static internal EventHandler<Game.Roster> SyncMoveLabelsHandler = (object? _, Game.Roster rostId) =>
    {
        if (!rosterToLabMgr.TryGetValue(rostId, out LabelManager.Info stateMove))
            throw new Exception($"VBridge.SyncMoveLabelsHandler : can't translate '{rostId}'");

        EM.Raise(EM.Evt.UpdateLabels, new { }, new Enum[] { stateMove });
    };
}
