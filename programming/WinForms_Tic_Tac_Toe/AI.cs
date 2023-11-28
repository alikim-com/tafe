
namespace WinFormsApp1;

public class AI
{
    public enum Logic
    {
        None,
        RNG,
        Easy
    }

    readonly Logic logic;
    readonly Game.Roster rosterId;

    public AI(Logic _logic, Game.Roster _rosterId)
    {
        logic = _logic;
        rosterId = _rosterId;
    }

    /// <summary>
    /// Choose a board cell
    /// </summary>
    /// <param name="count">The number of remaining UI elements to click</param>
    public EventHandler<Game.Roster> AIMakeMoveHandler()
    {
        return logic switch
        {
            Logic.RNG =>
            (object? _, Game.Roster curPlayer) =>
            {
                Thread thread = new(() =>
                {
                    Thread.Sleep(750);

                    Random random = new();
                    EM.InvokeFromMainThread(() => EM.Raise(EM.Evt.AIMoved, new { }, Point.Empty));
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

                    Thread.Sleep(250); // to see the reason for the decision in case AI vs AI

                    EM.InvokeFromMainThread(() => EM.Raise(EM.Evt.AIMoved, new { }, move));
                });

                thread.Start();
            }
            ,
            _ => throw new NotImplementedException($"AI.AIMakeMoveHandler : logic '{logic}' not supported"),
        };
        ;
    }

    /// <summary>
    /// Simple AI logic (esay mode) for playing the game
    /// </summary>
    static Point LogicEasy()
    {
        static bool CanTake(Point pnt) => Game.Board[pnt.X, pnt.Y] == Game.Roster.None;

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
            foreach (var rec in stat)
            {
                if (rec.Value == 2 && rec.Key != Game.Roster.None && free.Count > 0)
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
