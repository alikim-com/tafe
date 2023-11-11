
namespace WinFormsApp1;

/// <summary>
/// The purpose is to go thru TurnList, identify a player as a Human or AI;<br/>
/// in case of AI, run logic for choosing a move and performing clicks,<br/>
/// in case of a Human player, wait for their action
/// </summary>
internal class TurnWheel
{
    static AppForm? AppFormInstance;

    static int head;

    public enum Mode
    {
        Config, // AI chooses panels
        Play // AI plays the game
    }

    public enum Repeat // go thru TurnList
    {
        Once, // for player cfg
        Loop, // for playing game
    }

    static Mode mode;
    static Repeat repeat;

    static List<IComponent> uiChoice = new();

    public static Game.Roster CurPlayer => Game.TurnList[head];

    static public void Reset()
    {
        head = 0;
        uiChoice = new();
    }

    static public void Start(AppForm _inst, List<IComponent> _uiChoice, Mode _mode)
    {
        AppFormInstance = _inst;
        uiChoice = _uiChoice;
        mode = _mode;
        repeat = _mode == Mode.Config ? Repeat.Once : Repeat.Loop;

        AssertPlayer();

        EM.EvtPlayerConfigured += Next;
        EM.EvtAIConfigMoved += AIConfigMoved;
    }

    /// <summary>
    /// Handles EM.EvtAIConfigMoved event sent when AI chose a panel to click on
    /// </summary>
    /// <param name="e">index in the range [0, uiChoice.Count)</param>
    private static void AIConfigMoved(object? sender, int e) => uiChoice[e].SimulateOnClick();

    static public void Stop()
    {
        EM.EvtPlayerConfigured -= Next;
        EM.EvtAIConfigMoved -= AIConfigMoved;
    }

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

        ComponentHandler(comp);

        uiChoice.Remove(comp);

        if (uiChoice.Count == 0)
        {
            Ended(); // outside call
            return;
        }

        AdvancePlayer();

        AssertPlayer();
    }

    /// <summary>
    /// Custom logic re how to visualize selected components 
    /// and if they need to be disabled
    /// </summary>
    static void ComponentHandler(IComponent comp)
    {
        if (CurPlayer.ToString().StartsWith("Human")) comp.Highlight();
        comp.Disable();
    }

    static void AdvancePlayer()
    {
        if (head == Game.TurnList.Length - 1)
            if (repeat == Repeat.Once)
            {
                Ended();
                return;
            }
            else if (repeat == Repeat.Loop)
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

            if (mode == Mode.Config)
                AI.MakeConfigMove(uiChoice.Count);
            else
                AI.MakePlayMove();
        }
        else
        { // Human*

            EnableAll();
        }
    }

    static void Ended()
    {

    }

}
