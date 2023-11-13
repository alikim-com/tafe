
namespace WinFormsApp1;

/// <summary>
/// Game engine, controls players, moves and the board
/// </summary>
internal class Game
{
    /// <summary>
    /// Pattern for human players Human*, for AI players - AI*
    /// </summary>
    public enum Roster
    {
        None,
        Human,
        AI
    }

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

    public static Roster[,] board = new Roster[3, 3];

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
        // also allows to save/load games
        EM.RaiseEvtSyncBoard(new { }, update);
        
        // reset everything, new game
        EM.RaiseEvtReset(new { }, new EventArgs());
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

}
