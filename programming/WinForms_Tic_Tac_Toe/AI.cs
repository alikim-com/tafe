
using System;

namespace WinFormsApp1;

internal class AI
{
    public enum Logic
    {
        ConfigRNG,
        BoardRNG,
        BoardEasy
    }

    static readonly Dictionary<Logic, Action<int, Logic>> action = new()
    {
        { Logic.ConfigRNG, MakeConfigMove },
        { Logic.BoardRNG, MakeBoardMove },
        { Logic.BoardEasy, MakeBoardMove },
    };

    /// <summary>
    /// Called from TurnWheel to choose a UI element
    /// </summary>
    public static void MakeMove(int count, Logic L) => action[L](count, L);

    /// <summary>
    /// Choose a config panel
    /// </summary>
    /// <param name="count">The number of remaining UI elements to click</param>
    public static void MakeConfigMove(int count, Logic L)
    {
        Thread thread = new(() =>
        {
            Thread.Sleep(1000);

            switch (L)
            {
                case Logic.ConfigRNG:
                    {
                        Random random = new();
                        EM.InvokeFromMainThread(() => EM.Raise(EM.Evt.AIMoved, new { }, random.Next(count)));
                        break;
                    }

                default:
                    throw new NotImplementedException($"AI.MakeConfigMove : logic '{L}'");
            }

        });

        thread.Start();
    }

    /// <summary>
    /// Choose a board cell
    /// </summary>
    /// <param name="count">The number of remaining UI elements to click</param>
    public static void MakeBoardMove(int count, Logic L)
    {
        Thread thread = new(() =>
        {
            Thread.Sleep(1000);

            switch (L)
            {
                case Logic.BoardRNG:
                    {
                        Random random = new();
                        EM.InvokeFromMainThread(() => EM.Raise(EM.Evt.AIMoved, new { }, random.Next(count)));
                        break;
                    }

                case Logic.BoardEasy:
                    {
                        int move = LogicEasy();
                        EM.InvokeFromMainThread(() => EM.Raise(EM.Evt.AIMoved, new { }, move));
                    }
                    break;
                default:
                    throw new NotImplementedException($"AI.MakeConfigMove : logic '{L}'");
            }

        });

        thread.Start();
    }

    /// <summary>
    /// Simple logic for playing the game
    /// </summary>
    static int LogicEasy()
    {
        Game.Roster self = TurnWheel.CurPlayer;

        // block the opponent
        foreach(var line in Game.Lines)
        {
            if (line[0] == line[1]) { }

        }

        return 0;
    }
}
