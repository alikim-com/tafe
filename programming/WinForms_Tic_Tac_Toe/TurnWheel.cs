
using static WinFormsApp1.LabelManager;

namespace WinFormsApp1;

/// <summary>
/// The purpose is to go thru TurnList, identify a player as a Human or AI;<br/>
/// in case of AI, trigger logic for choosing a move and performing clicks,<br/>
/// in case of a Human player, wait for their action.<br/>
/// Update labels.
/// </summary>
internal class TurnWheel
{
    static int head;

    public enum Repeat // cycle thru TurnList
    {
        Once, // for player cfg
        Loop, // for playing game
    }

    static AI.Logic mode;
    static Repeat repeat;

    /// <summary>
    /// Special case AI vs Human head to head, to set custom labels
    /// </summary>
    static bool AvH;
    /// <summary>
    /// AvH case, final cfg label
    /// </summary>
    static Choice cfgEndedLabel;
    /// <summary>
    /// AvH case, human confirm label
    /// </summary>
    static readonly Enum[] cfgConfirmLabel = PlayerIsAI(0) ?
        new Enum[] { Choice.AIFirst } : new Enum[] { Choice.HumanFirst };

    /// <summary>
    /// Clickable UI elements
    /// </summary>
    static List<IComponent> uiChoice = new();

    public static Game.Roster CurPlayer => Game.TurnList[head];

    static bool CheckPlayerType(Game.Roster player, string type) => player.ToString().StartsWith(type);
    static bool CurPlayerIsHuman => CheckPlayerType(CurPlayer, "Human");
    static bool CurPlayerIsAI => CheckPlayerType(CurPlayer, "AI");
    static bool PlayerIsHuman(int ind) => CheckPlayerType(Game.TurnList[ind], "Human");
    static bool PlayerIsAI(int ind) => CheckPlayerType(Game.TurnList[ind], "AI");

    static public void Reset()
    {
        head = 0;
        uiChoice = new();
    }

    /// <summary>
    /// Handles EvtAIMoved event sent after AI chooses a UI element to click on
    /// </summary>
    /// <param name="e">index in the range [0, uiChoice.Count)</param>
    private static EventHandler<int> AIMoved = 
        (object? sender, int e) => uiChoice[e].SimulateOnClick();

    static public void Start(List<IComponent> _uiChoice, AI.Logic _mode)
    {
        uiChoice = _uiChoice;
        mode = _mode;
        repeat = _mode == AI.Logic.Config ? Repeat.Once : Repeat.Loop;

        AvH = Game.TurnList.Length == 2 &&
            (PlayerIsHuman(0) && PlayerIsAI(1) || PlayerIsHuman(1) && PlayerIsAI(0));

        // translate AI responce to a click on a UI element
        EM.Unsubscribe(EM.Evt.AIMoved, AIMoved);
        EM.Subscribe(EM.Evt.AIMoved, AIMoved);
        
        AssertPlayer();
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
    /// Subscribed to cfg panels clicks
    /// </summary>
    /// <param name="_">BgMode is not used here;<br/>
    /// used instead by VBridge, which is also subscribed.
    /// </param>
    static public EventHandler<CellWrapper.BgMode> PlayerConfiguredHandler = 

    (object? s, CellWrapper.BgMode _) =>
    {
        if (s == null) return;
        IComponent comp = (IComponent)s;

        if (AvH && comp.Name == "pLeft")
            cfgEndedLabel = CurPlayerIsHuman ? Choice.HumanLeft : Choice.HumanRight;

        if (CurPlayerIsHuman) comp.Highlight();
        //comp.Disable();

        Advance(comp);
    };

    /// <summary>
    /// Turns the wheel: asserts remaining active UI elements to click,<br/>
    /// ensures the next player is active and ready to click
    /// </summary>
    /// <param name="comp">Clicked component</param>
    static public void Advance(IComponent comp) 
    {
        uiChoice.Remove(comp);

        if (uiChoice.Count == 0)
        {
            Ended(); // outside call
            return;
        }

        AdvancePlayer();

        AssertPlayer();
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
        if (AvH && mode == AI.Logic.Config)
            EM.Raise(EM.Evt.UpdateLabels, new { }, cfgConfirmLabel);

        if (CurPlayerIsAI)
        {
            DisableAll();

            AI.MakeMove(mode, uiChoice.Count);
            EM.Raise(EM.Evt.UpdateLabels, new { }, new Enum[] { Info.AITurn });
        }
        else
        { // Human*

            EnableAll();
            EM.Raise(EM.Evt.UpdateLabels, new { }, new Enum[] { Info.HumanTurn });
        }
    }

    static void Ended()
    {
        if (mode == AI.Logic.Config)
        {
            EM.Raise(EM.Evt.UpdateLabels, new { }, new Enum[] { cfgEndedLabel, Info.None });
            GameCountdown();

        }
        else
        {

            MessageBox.Show("TurnWheel.Ended : GAME OVER");

        }

    }

    static void GameCountdown()
    {
        Thread thread = new(CntDown);
        thread.Start();
    }

    static void CntDown()
    {
        Thread.Sleep(500);
        foreach (Countdown e in Enum.GetValues(typeof(Countdown)))
        {
            // raise from UI thread for safe UI access
            EM.InvokeFromMainThread(() => EM.Raise(EM.Evt.UpdateLabels, new { }, new Enum[] { e }));
            Thread.Sleep(1000);
        }

        // UI safety
        EM.InvokeFromMainThread(() => EM.Raise(EM.Evt.ConfigFinished, new { }, new EventArgs()));
    }

}
