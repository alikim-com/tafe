
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
    static bool busy;

    public static Game.Roster CurPlayer => Game.TurnList[head];

    static bool CheckPlayerType(Game.Roster player, string type) => player.ToString().StartsWith(type);
    static bool CurPlayerIsHuman => CheckPlayerType(CurPlayer, "Human");
    static bool CurPlayerIsAI => CheckPlayerType(CurPlayer, "AI");
    static bool PlayerIsHuman(int ind) => CheckPlayerType(Game.TurnList[ind], "Human");
    static bool PlayerIsAI(int ind) => CheckPlayerType(Game.TurnList[ind], "AI");

    static Action EnableUICb = () => { };
    static Action DisableUICb = () => { };

    static public void SetCallbacks(Action _EnableUICb, Action _DisableUICb)
    {
        EnableUICb = _EnableUICb;
        DisableUICb = _DisableUICb;
    }

    static public void Reset()
    {
        head = -1;
        busy = false;
    }

    public static readonly EventHandler<Point> PlayerMovedHandler = (object? sender, Point e) =>
    {
        if (busy) return;
        busy = true;

        if (sender is not IComponent iComp)
            throw new Exception($"TurnWheel.PlayerMovedHandler : '{sender}' is not IComponent");

        iComp.IsLocked = true;
        iComp.Disable();


    };

    static void Advance() 
    {
        AdvancePlayer();

        AssertPlayer();
    }

    static void AdvancePlayer() => head = head == Game.TurnList.Length - 1 ? 0 : head + 1;

    /// <summary>
    /// Ensure next click is scheduled and will be performed
    /// </summary>
    static void AssertPlayer()
    {
        //if (AvH && mode == AI.Logic.ConfigRNG)
        //    EM.Raise(EM.Evt.UpdateLabels, new { }, cfgConfirmLabel);

        //if (CurPlayerIsAI)
        //{
        //    DisableAll();

        //    AI.MakeMove(uiChoice.Count, mode);
        //    //EM.Raise(EM.Evt.UpdateLabels, new { }, new Enum[] { Info.AITurn });
        //}
        //else
        //{ // Human*

        //    EnableAll();
        //   // EM.Raise(EM.Evt.UpdateLabels, new { }, new Enum[] { Info.HumanTurn });
        //}
    }

    static public void GameCountdown()
    {
        Thread thread = new(CntDown);
        thread.Start();
    }

    static void CntDown()
    {
        Thread.Sleep(500);
        foreach (LabelManager.Countdown e in Enum.GetValues(typeof(LabelManager.Countdown)))
        {
            // raise from UI thread for safe UI access
            EM.InvokeFromMainThread(() => EM.Raise(EM.Evt.UpdateLabels, new { }, new Enum[] { e }));
            Thread.Sleep(1000);
        }

        // UI safety
        EM.InvokeFromMainThread(Advance);
    }

}
