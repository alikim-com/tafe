
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

    static List<IComponent> uiChoice = new();

    public static Game.Roster CurPlayer => Game.TurnList[head];

    static public void Reset()
    {
        head = 0;
        uiChoice = new();
    }

    static public void Start(List<IComponent> _uiChoice, Mode _mode)
    {
        uiChoice = _uiChoice;
        mode = _mode;

        AssertPlayer();

        EM.EvtPlayerConfigured += Next;
    }

    static public void Stop() => EM.EvtPlayerConfigured -= Next;

    static void EnableAll()
    {
        foreach (IComponent e in uiChoice) e.Enable();
    }

    static void DisableAll()
    {
        foreach (IComponent e in uiChoice) e.Disable();
    }

    /// <summary>
    /// Subscribed to UI click events
    /// </summary>
    static public void Next(object? s, CellWrapper.BgMode _)
    {
        if (s == null) return;
        IComponent comp = (IComponent)s;

        OnClickHandler(comp);

        uiChoice.Remove(comp);

        if (uiChoice.Count == 0)
        {
            Ended(); // outside call
            return;
        }

        AdvancePlayer();

        AssertPlayer();
    }

    static void OnClickHandler(IComponent comp)
    {
        if (CurPlayer.ToString().StartsWith("Human")) comp.Highlight();
        comp.Disable();
    }

    static void AdvancePlayer()
    {
        if (head == Game.TurnList.Length - 1)
            if (mode == Mode.Once)
            {
                Ended();
                return;
            }
            else if (mode == Mode.Loop)
            {
                head = -1;
            }

        head++;
    }

    /// <summary>
    /// Ensure next click is scheduled and will be performed
    /// </summary>
    static void AssertPlayer()
    {
        MessageBox.Show(CurPlayer.ToString());

        if (CurPlayer.ToString().StartsWith("AI"))
        {
            DisableAll();

            // thread logic   <---- how to click

        } else { // Human*

            EnableAll();
        }
    }

    static void Ended()
    {
        EM.EvtPlayerConfigured += Next;
        
        // Start({cells}, Loop)
    }

}
