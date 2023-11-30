
namespace WinFormsApp1;

class AI
{
    internal enum Logic
    {
        None,
        RNG,
        Easy
    }

    readonly Logic logic;
    readonly Game.Roster rosterId;

    internal AI(Logic _logic, Game.Roster _rosterId)
    {
        logic = _logic;
        rosterId = _rosterId;
    }

    /// <summary>
    /// Choose a board cell
    /// </summary>
    /// <param name="count">The number of remaining UI elements to click</param>
    internal EventHandler<Game.Roster> AIMakeMoveHandler()
    {
        return logic switch
        {
            Logic.RNG =>
            (object? _, Game.Roster curPlayer) =>
            {
                Thread thread = new(() =>
                {
                    Thread.Sleep(750);

                    Point move = LogicRNG();

                    EM.InvokeFromMainThread(() => EM.Raise(EM.Evt.AIMoved, new { }, move));
                });

                thread.Start();
            }
            ,
            Logic.Easy => 
            (object? _, Game.Roster curPlayer) =>
            {
                Thread thread = new(() =>
                {
                    Thread.Sleep(750);

                    Point move = LogicEasy();

                    Thread.Sleep(250);

                    EM.InvokeFromMainThread(() => EM.Raise(EM.Evt.AIMoved, new { }, move));
                });

                thread.Start();
            }
            ,
            _ => throw new NotImplementedException($"AI.AIMakeMoveHandler : logic '{logic}' not supported"),
        };
        ;
    }

    static bool CanTake(Point pnt) => Game.board[pnt.X, pnt.Y] == Game.Roster.None;

    static Point LogicRNG()
    {
        var canTake = new List<Point>();

        for (int i = 0; i < Game.board.width; i++)
            for (int j = 0; j < Game.board.height; j++)
            {
                var pnt = new Point(i, j);
                if(CanTake(pnt)) canTake.Add(pnt);
            }

        if (canTake.Count == 0)
            throw new Exception("AI.LogicRNG : run on full board");

        var rng = new Random();
        var choice = rng.Next(canTake.Count);

        return canTake[choice];
    }

    /// <summary>
    /// Simple AI logic (esay mode) for playing the game
    /// </summary>
    static Point LogicEasy()
    {
        // examine lines
        foreach (var line in Game.lines)
        {
            // gather stats for the line
            Dictionary<Game.Roster, int> stat = new();

            var canTake = new List<Point>();

            foreach (var pnt in line)
            {
                //var player = Game.board[pnt.X, pnt.Y];
                //if (!stat.ContainsKey(player)) stat.Add(player, 0);
                //stat[player]++;
                //if (player == Game.Roster.None) free.Add(pnt);
            }
            // search the line for 2 cells taken by same player
            foreach (var rec in stat)
            {
                //if (rec.Value == 2 && rec.Key != Game.Roster.None && free.Count > 0)
                    // block player (rec.Key != self) or
                    // win the game (rec.Key == self)

                    // TODO send labels update

                    //return LinearOffset(free[0]);
                    return Point.Empty;
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
                //return LinearOffset(spot);
                return Point.Empty;
            }


        return Point.Empty;
    }
}
