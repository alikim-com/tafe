
namespace WinFormsApp1;

internal class AI
{
    public enum Logic
    {
        RNG,
        Easy
    }

    static readonly Dictionary<Logic, Action<int, Logic>> action = new()
    {
        { Logic.RNG, MakeBoardMove },
        { Logic.Easy, MakeBoardMove },
    };

    /// <summary>
    /// Called from TurnWheel to choose a UI element
    /// </summary>
    public static void MakeMove(int count, Logic L) => action[L](count, L);

    /// <summary>
    /// Choose a board cell
    /// </summary>
    /// <param name="count">The number of remaining UI elements to click</param>
    public static void MakeBoardMove(int count, Logic L)
    {
        Thread thread = new(() =>
        {
            Thread.Sleep(750);

            switch (L)
            {
                case Logic.RNG:
                    {
                        Random random = new();
                        EM.InvokeFromMainThread(() => EM.Raise(EM.Evt.AIMoved, new { }, random.Next(count)));
                        break;
                    }

                case Logic.Easy:
                    {
                        int move = LogicEasy();
                        
                        Thread.Sleep(250); // to see the reason for the decision in case AI vs AI

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
    /// Simple AI logic (esay mode) for playing the game
    /// </summary>
    static int LogicEasy()
    {
        static bool CanTake(Point pnt) => Game.Board[pnt.X, pnt.Y] == Game.Roster.None;

        static int LinearOffset(Point pnt) => pnt.X * Game.boardSize.Width + pnt.Y;

        // examine lines
        foreach (var line in Game.Lines)
        {
            // gather stats for the line
            Dictionary<Game.Roster, int> stat = new();
            // memo free cells
            List<Point> free = new();
            foreach (var pnt in line)
            {
                var player = Game.Board[pnt.X, pnt.Y];
                if (!stat.ContainsKey(player)) stat.Add(player, 0);
                stat[player]++;
                if (player == Game.Roster.None) free.Add(pnt);
            }
            // search the line for 2 cells taken by same player
            foreach(var rec in stat)
            {
                if (rec.Value == 2 && rec.Key != Game.Roster.None && free.Count > 0)
                    // block player (rec.Key != self) or
                    // win the game (rec.Key == self)

                    // TODO send labels update
                    
                    return LinearOffset(free[0]);
            }

        }

        // try to take best spots
        List<Point> bestSpots = new()
        { 
            // middle
            new Point(1,1),
            // corners
            new Point(0,0),
            new Point(0,2),
            new Point(2,0),
            new Point(2,2),
        };
        foreach (var spot in bestSpots)
            if (CanTake(spot))
            {
                // TODO send labels
                return LinearOffset(spot);
            }


        return 0;
    }
}
