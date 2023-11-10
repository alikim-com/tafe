
namespace WinFormsApp1;

/// <summary>
/// Accepts player config input in the order of Game.TurnList<br/>
/// to help init VBridge
/// </summary>
internal class PlayerConfig
{
    static int head;
    public static Dictionary<Game.Roster, CellWrapper.BgMode> toCellCfg = new();

    /// <summary>
    /// Subscribed to EM.EvtReset event
    /// </summary>
    public static void ResetHandler(object? s, EventArgs e)
    {
        head = 0;
        toCellCfg = new();
    }
    /// <summary>
    /// Adds an association between a player and a cell's visual to the dictionary
    /// </summary>
    public static void PlayerConfirmedHandler(object? s, CellWrapper.BgMode e)
    {
        toCellCfg.Add(Game.TurnList[head], e);
        AdvancePlayer();
    }

    static void AdvancePlayer()
    {
        if(head == Game.TurnList.Length - 1) ConfigComplete();
        head++;
    }

    static void ConfigComplete()
    {
        VBridge.AddCfg(toCellCfg);

        // raise an event to start player turns
    }
}
