
namespace WinFormsApp1;

/// <summary>
/// Defines players roster, evaluates the board and winning conditions
/// </summary>
public class Game
{
    /// <summary>
    /// Pattern for human player names Human*, for AI players - AI*
    /// </summary>
    public enum Roster
    {
        None,
        Human_One,
        Human_Two,
        AI_One,
        AI_Two
    }

    static public Dictionary<Roster, string> rosterIdentity = new()
    {
        { Roster.Human_One, "Ironheart" },
        { Roster.Human_Two, "Silverlight" },
        { Roster.AI_One, "Quantum" },
        { Roster.AI_Two, "Syncstorm" }
    };

    static Roster[] _turnList = 
        ((IEnumerable<Roster>)Enum.GetValues(typeof(Roster)))
        .Where(e => e != Roster.None).ToArray(); // exclude None

    /// <summary>
    /// Players from Roster in the order of their turns;<br/>
    /// can be overwritten by SetTurns()
    /// </summary>
    static public Roster[] TurnList
    {
        get => _turnList;
        set => _turnList = value;
    }

    public static Roster _curPlayer;
    public static Roster CurPlayer
    {
        get => _curPlayer;
        private set => _curPlayer = value;
    }

    static readonly Roster[,] board = new Roster[3, 3];
    public static Size boardSize = new(board.GetLength(0), board.GetLength(1));
    public static Roster[,] Board => board;

    /// <summary>
    /// Utility array Point(row, col) to help assert winning conditions and to assist AI logic
    /// </summary>
    static readonly Point[][] lines = new Point[][]
    {
        // horizontal
       new Point[] { new Point(0,0), new Point(0,1), new Point(0,2) },
       new Point[] { new Point(1,0), new Point(1,1), new Point(1,2) },
       new Point[] { new Point(2,0), new Point(2,1), new Point(2,2) },

       // vertical
       new Point[] { new Point(0,0), new Point(1,0), new Point(2,0) },
       new Point[] { new Point(0,1), new Point(1,1), new Point(2,1) },
       new Point[] { new Point(0,2), new Point(1,2), new Point(2,2) },

       // diagonal
       new Point[] { new Point(0,0), new Point(1,1), new Point(2,2) },
       new Point[] { new Point(0,2), new Point(1,1), new Point(0,2) },
    };
    public static Point[][] Lines => lines;

    public static void Reset()
    {
        CurPlayer = Roster.None;
        ResetBoard();

        // add all the board cells to the update
        var update = new Dictionary<Point, Roster>();
        for (int i = 0; i < board.GetLength(0); i++)
            for (int j = 0; j < board.GetLength(1); j++)
                update.Add(new Point(i, j), board[i, j]);

        // sync the board
        // also allows to save/load games TODO
        EM.Raise(EM.Evt.SyncBoard, new { }, update);
        
        // reset everything, new game
        EM.Raise(EM.Evt.Reset, new { }, new EventArgs());
    }

    /// <summary>
    /// Sets the order of players turns
    /// </summary>
    public static void SetTurns(string mode)
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
        for (int i = 0; i < board.GetLength(0); i++)
            for (int j = 0; j < board.GetLength(1); j++)
                board[i, j] = Roster.None;
    }

    static void AssertGame(Point rc, object s)
    {
        
        // assert the game state

        // if game over, call TurnWheel.Ended();
        // return;

        // else, turn the wheel
        TurnWheel.Advance((IComponent)s);
    }

    /// <summary>
    /// Subscribed to cell click event;<br/>
    /// asserts the game state (win/loss), issues board sync event, turns the wheel
    /// </summary>
    static public EventHandler<Point> PlayerMovedHandler = (object? s, Point rc) =>
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
