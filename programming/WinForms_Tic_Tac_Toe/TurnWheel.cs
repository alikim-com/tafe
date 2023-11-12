
using static WinFormsApp1.LabelManager;

namespace WinFormsApp1;

/// <summary>
/// The purpose is to go thru TurnList, identify a player as a Human or AI;<br/>
/// in case of AI, run logic for choosing a move and performing clicks,<br/>
/// in case of a Human player, wait for their action
/// </summary>
internal class TurnWheel
{
    static int head;

    public enum Repeat // go thru TurnList
    {
        Once, // for player cfg
        Loop, // for playing game
    }

    static AI.Logic mode;
    static Repeat repeat;

    static List<IComponent> uiChoice = new();

    public static Game.Roster CurPlayer => Game.TurnList[head];

    static public void Reset()
    {
        head = 0;
        uiChoice = new();
    }

    static public void Start(List<IComponent> _uiChoice, AI.Logic _mode)
    {
        uiChoice = _uiChoice;
        mode = _mode;
        repeat = _mode == AI.Logic.Config ? Repeat.Once : Repeat.Loop;

        AssertPlayer();

        EM.EvtPlayerConfigured += Next;
        EM.EvtAIMoved += AIMoved;
    }

    /// <summary>
    /// Handles EM.EvtAIConfigMoved event sent when AI chose a panel to click on
    /// </summary>
    /// <param name="e">index in the range [0, uiChoice.Count)</param>
    private static void AIMoved(object? sender, int e) => uiChoice[e].SimulateOnClick();

    static public void Stop()
    {
        EM.EvtPlayerConfigured -= Next;
        EM.EvtAIMoved -= AIMoved;
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

    // custom two player case
    static readonly string firstPlayer = 
        Game.TurnList[0].ToString().StartsWith("AI") ? "AI" :"HU";
    static readonly Enum[] labels = firstPlayer == "AI" ?
        new Enum[] { Choice.AIFirst } : new Enum[] { Choice.HumanFirst };

    /// <summary>
    /// Ensure next click is scheduled and will be performed
    /// </summary>
    static void AssertPlayer()
    {
        // custom two player case
        if (mode == AI.Logic.Config) EM.RaiseEvtUpdateLabels(new { }, labels);

        if (CurPlayer.ToString().StartsWith("AI"))
        {
            DisableAll();

            AI.MakeMove(mode, uiChoice.Count);
            EM.RaiseEvtUpdateLabels(new { }, new Enum[] { Info.AITurn });
        }
        else
        { // Human*

            EnableAll();
            EM.RaiseEvtUpdateLabels(new { }, new Enum[] { Info.HumanTurn });
        }
    }

    static void Ended()
    {
        //  if config set -vs- label
        // EM.RaiseEvtUpdateLabels(new { }, new Enum[] { LabelManager.Info.HumanTurn });
        // else None
        EM.RaiseEvtConfigFinished(new { }, new EventArgs());
    }

}
