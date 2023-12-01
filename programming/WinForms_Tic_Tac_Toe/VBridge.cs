
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
    static readonly Dictionary<ChoiceItem.Side, CellWrapper.BgMode> sideToBg = new()
    {
        { ChoiceItem.Side.None, CellWrapper.BgMode.Default },
        { ChoiceItem.Side.Left, CellWrapper.BgMode.Player1 },
        { ChoiceItem.Side.Right, CellWrapper.BgMode.Player2 }
    };

    // LabelManager

    /// <summary>
    /// Auxiliary map
    /// </summary>
    static readonly Dictionary<ChoiceItem.Side, LabelManager.Info> sideToInfoLab = new()
    {
        { ChoiceItem.Side.Left, LabelManager.Info.Player1 },
        { ChoiceItem.Side.Right, LabelManager.Info.Player2 }
    };

    /// <summary>
    /// Memorise players' sides for mapping them into cell bgs & message labels
    /// </summary>
    static readonly Dictionary<Game.Roster, ChoiceItem.Side> rosterToSide = new();

    /// <summary>
    /// Fill in LabelManager.stateToString messages that depend on player names and their sides
    /// </summary>
    static internal void Reset(IEnumerable<ChoiceItem> chosen)
    {
        rosterToSide.Clear();
        rosterToSide.Add(Game.Roster.None, ChoiceItem.Side.None);

        Dictionary<LabelManager.Info, string> playerInfo = new();

        foreach (var chItem in chosen)
        {
            var side = chItem.side;

            rosterToSide.Add(chItem.rosterId, side);

            var state = Utils.SafeDictValue(sideToInfoLab, side);
            var stateMove = Utils.SafeEnumFromStr<LabelManager.Info>($"{state}Move");
            var stateWon = Utils.SafeEnumFromStr<LabelManager.Info>($"{state}Won");

            var msgMove = chItem.originType == "AI" ?
                $"{chItem.identityName} is moving..." : $"Your move, {chItem.identityName}...";

            var msgWon = $"Player {chItem.identityName} is the winner! Congratulations!";

            playerInfo.Add(state, chItem.identityName);
            playerInfo.Add(stateMove, msgMove);
            playerInfo.Add(stateWon, msgWon);
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
    static internal EventHandler<Dictionary<Tile, Game.Roster>> SyncBoardHandler =
    (object? s, Dictionary<Tile, Game.Roster> e) =>
    {
        Dictionary<Point, CellWrapper.BgMode> cellBgs = new();

        foreach (var (rc, rostId) in e)
        {
            var side = Utils.SafeDictValue(rosterToSide, rostId);
            var bg = Utils.SafeDictValue(sideToBg, side);
            cellBgs.Add(new Point(rc.row, rc.col), bg);
        }

        EM.Raise(EM.Evt.SyncBoardUI, s ?? new { }, cellBgs);
    };

    /// <summary>
    /// Subscribed to EM.SyncMoveLabels event raised by TurnWheel to update labels on player move
    /// </summary>
    static internal EventHandler<Game.Roster> SyncMoveLabelsHandler =
    (object? _, Game.Roster rostId) =>
    {
        var side = Utils.SafeDictValue(rosterToSide, rostId);
        var state = Utils.SafeDictValue(sideToInfoLab, side);
        var stateMove = Utils.SafeEnumFromStr<LabelManager.Info>($"{state}Move");

        EM.Raise(EM.Evt.UpdateLabels, new { }, new Enum[] { stateMove });
    };

    static internal EventHandler<Game.Roster> GameOverHandler =
    (object? _, Game.Roster winner) =>
    {
        var side = Utils.SafeDictValue(rosterToSide, winner);
        var state = Utils.SafeDictValue(sideToInfoLab, side);
        var stateMove = Utils.SafeEnumFromStr<LabelManager.Info>($"{state}Move");

        EM.Raise(EM.Evt.UpdateLabels, new { }, new Enum[] { stateMove });
    };
}
