
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
    readonly Game.Roster selfId;

    internal AI(Logic _logic, Game.Roster _selfId)
    {
        logic = _logic;
        selfId = _selfId;
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

     static Point LogicRNG()
    {
        var canTake = new List<Point>();

        for (int i = 0; i < Game.board.width; i++)
            for (int j = 0; j < Game.board.height; j++)
            {
                var pnt = new Point(i, j);
                if(Game.CanTakeBoardTile(pnt, out Game.Roster _)) canTake.Add(pnt);
            }

        if (canTake.Count == 0)
            throw new Exception("AI.LogicRNG : run on full board");

        var rng = new Random();
        var choice = rng.Next(canTake.Count);

        return canTake[choice];
    }

    class LineInfo
    {
        readonly Line line;
        internal readonly List<int> canTake = new();
        readonly Dictionary<Game.Roster, int> taken = new();
        int selfTaken = 0;

        internal LineInfo(Line _line)
        {
            line = _line;
        }
    }

    /// <summary>
    /// Simple AI logic (esay mode) for playing the game
    /// </summary>
    static Point LogicEasy()
    {
        var linesInfo = new List<LineInfo>();

        // examine lines
        foreach (var line in Game.lines)
        {
            var info = new LineInfo(line);

            for (int i = 0; i < line.Length; i++)
                if (Game.CanTakeLineTile(line, i, out Game.Roster player))
                {
                    info.canTake.Add(i);
                }else
                {

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
