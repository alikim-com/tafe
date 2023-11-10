
namespace WinFormsApp1;

/// <summary>
/// The purpose is to go thru TurnList, identify a player as a Human or AI;<br/>
/// in case of AI, run logic for choosing a move and performing clicks,<br/>
/// in case of a Human player, wait for their action
/// </summary>
internal class TurnWheel
{
    static int head;
    
    public enum Mode
    {
        Once, // for player cfg
        Loop, // for playing game
    }

    static Mode mode;

    static List<IComponent?>? uiChoice;

    public static Game.Roster CurPlayer => Game.TurnList[head];

    static public void Reset()
    {
        head = -1;
        uiChoice = null;
    }

    static public void Start(List<IComponent?> _uiChoice, Mode _mode) 
    {
        uiChoice = _uiChoice;
        mode = _mode;

        EM.EvtPlayerConfigured += Next;
    }

    static public void Stop() => EM.EvtPlayerConfigured -= Next;

    /// <summary>
    /// Subscribed to UI click events
    /// </summary>
    static public void Next(object? s, CellWrapper.BgMode e)
    {
        head++;
        if (CurPlayer.ToString().StartsWith("AI"))
        {

        }
    }


}
