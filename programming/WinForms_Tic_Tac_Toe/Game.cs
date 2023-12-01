
namespace WinFormsApp1;

/// <summary>
/// Defines players roster, evaluates the board and winning conditions
/// </summary>
class Game
{
    /// <summary>
    /// Pattern for human player names Human*, for AI players - AI*
    /// </summary>
    internal enum Roster
    {
        None,
        Human_One,
        Human_Two,
        AI_One,
        AI_Two
    }

    static internal readonly Dictionary<Roster, string> rosterIdentity = new()
    {
        { Roster.Human_One, "Ironheart" },
        { Roster.Human_Two, "Silverlight" },
        { Roster.AI_One, "Quantum" },
        { Roster.AI_Two, "Syncstorm" }
    };

    static Roster[] _turnList = Array.Empty<Roster>();

    /// <summary>
    /// Players from Roster in the order of their turns;<br/>
    /// can be overwritten by SetTurns()
    /// </summary>
    static internal Roster[] TurnList
    {
        get => _turnList;
        set => _turnList = value;
    }

    static internal readonly Board board = new(3, 3, Roster.None);

    static internal readonly Line[] lines = new Line[]
    {
        // rows
        new(board, new Tile[] { new(0,0), new(0,1), new(0,2) }),
        new(board, new Tile[] { new(1,0), new(1,1), new(1,2) }),
        new(board, new Tile[] { new(2,0), new(2,1), new(2,2) }),
        // columns
        new(board, new Tile[] { new(0,0), new(1,0), new(2,0) }),
        new(board, new Tile[] { new(0,1), new(1,1), new(2,1) }),
        new(board, new Tile[] { new(0,2), new(1,2), new(2,2) }),
        // diagonals
        new(board, new Tile[] { new(2,0), new(1,1), new(0,2) }), // Fwd
        new(board, new Tile[] { new(0,0), new(1,1), new(2,2) }), // Bwd

    };

    static internal readonly ArraySegment<Line> rows = new(lines, 0, 3);
    static internal readonly ArraySegment<Line> cols = new(lines, 3, 3);
    static internal readonly ArraySegment<Line> diags = new(lines, 6, 2);

    static internal bool CanTakeBoardTile(int index, out Roster owner)
    {
        owner = board[index];
        return owner == Roster.None;
    }
    static internal bool CanTakeBoardTile(Point pnt, out Roster owner)
    {
        owner = board[pnt.X, pnt.Y];
        return owner == Roster.None;
    }

    static internal bool CanTakeLineTile(Line line, int index, out Roster owner)
    {
        owner = line[index];
        return owner == Roster.None;
    }

    static internal void Reset(Roster[] turnlist)
    {
        TurnList = turnlist;

        ResetBoard();

        // add all the board cells to the update
        var update = new Dictionary<Point, Roster>();

        for (int i = 0; i < board.width; i++)
            for (int j = 0; j < board.height; j++)
                update.Add(new Point(i, j), board[i, j]);

        // sync the board
        EM.Raise(EM.Evt.SyncBoard, new { }, update);
    }

    /// <summary>
    /// Sets the order of players turns
    /// </summary>
    static internal void SetTurns(string mode)
    {
        switch (mode)
        {
            case "random":
                // random shuffle
                Array.Sort(_turnList, (x, y) => Guid.NewGuid().CompareTo(Guid.NewGuid()));
                break;
            default:
                throw new NotImplementedException($"Game.SetTurns : mode '{mode}'");
        }
    }

    static void ResetBoard()
    {
        for (int i = 0; i < board.Length; i++) board[i] = Roster.None;
    }

    static void AssertGame(Point rc, object s)
    {

        // assert the game state

        // if game over, call TurnWheel.Ended();
        // return;

        // else, turn the wheel
        //TurnWheel.Advance((IComponent)s);
    }

    /// <summary>
    /// Subscribed to cell click event;<br/>
    /// asserts the game state (win/loss), issues board sync event, turns the wheel
    /// </summary>
    static internal readonly EventHandler<Point> PlayerMovedHandler = (object? s, Point rc) =>
    {
        if (s == null) throw new Exception("Game.PlayerMovedHandler : cell is null");

        board[rc.X, rc.Y] = TurnWheel.CurPlayer;

        // add the cell to the update
        var update = new Dictionary<Point, Roster>()
        {
            { rc, board[rc.X, rc.Y] }
        };

        // sync the board
        EM.Raise(EM.Evt.SyncBoard, new { }, update);

        AssertGame(rc, s);
    };

}
