
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
                if (curPlayer != selfId) return;

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
                if (curPlayer != selfId) return;

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

    }

    static Point LogicRNG()
    {
        var canTake = new List<int>();

        for (int i = 0; i < Game.board.Length; i++)
            if (Game.CanTakeBoardTile(i, out Game.Roster _)) canTake.Add(i);

        if (canTake.Count == 0)
            throw new Exception("AI.LogicRNG : run on full board");

        var rng = new Random();
        var choice = rng.Next(canTake.Count);

        EM.InvokeFromMainThread(() => EM.Raise(EM.Evt.UpdateLabels, new { }, new Enum[] { LabelManager.AIMsg.Random }));

        var tile = Game.board.GetTile(choice);

        return new Point(tile.row, tile.col);
    }

    class LineInfo
    {
        internal readonly Line line;
        internal readonly List<int> canTake = new();
        internal readonly Dictionary<Game.Roster, int> takenStats = new();
        internal KeyValuePair<Game.Roster, int> dominant = new(Game.Roster.None, 0);

        internal LineInfo(Line _line)
        {
            line = _line;
        }
    }

    /// <summary>
    /// Simple AI logic (easy mode) for playing the game<br/>
    /// Each line is examined to determine the highest presence (cells taken) by a player<br/>
    /// AI participates in the first available (has cells that can be taken) line with the highest presence,<br/>
    /// whether it's its own (to win the game) or a foe's (to stop them from winning)
    /// </summary>
    Point LogicEasy()
    {
        var linesInfo = new List<LineInfo>();

        // examine lines
        foreach (var line in Game.lines)
        {
            var info = new LineInfo(line);

            for (int i = 0; i < line.Length; i++)
            {
                var canTake = Game.CanTakeLineTile(line, i, out Game.Roster player);

                if (canTake) info.canTake.Add(i);

                if (Game.TurnList.Contains(player)) // excludes Roster.None in this case
                {
                    if (!info.takenStats.ContainsKey(player)) info.takenStats.Add(player, 0);
                    info.takenStats[player]++;
                }
            }

            info.dominant = info.takenStats.MaxBy(rec => rec.Value);
        }

        var playableLines = linesInfo.Where(rec => rec.canTake.Count > 0).ToArray();

        var playLine = playableLines.MaxBy(rec => rec.dominant.Value) ??
            throw new Exception($"AI.LogicEasy : run on full board");

        var tile = playLine.line.GetTile(playLine.canTake.First());

        var msg = playLine.dominant.Key == selfId ? LabelManager.AIMsg.Attack : LabelManager.AIMsg.Defend;

        EM.InvokeFromMainThread(() => EM.Raise(EM.Evt.UpdateLabels, new { }, new Enum[] { msg }));

        return new Point(tile.row, tile.col);
    }
}
